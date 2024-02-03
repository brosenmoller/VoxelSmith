use std::{collections::HashMap, fs::File, io::Write};
use eframe::egui;
use egui::Color32;

use crate::{obj_generation, schematic::{self, Schematic, _dump_json}};
use schematic::load_schematic;
use obj_generation::generate_obj_string;

#[derive(Debug, PartialEq, Eq, Hash)]
enum AppError {
    SchematicLoad,
}

pub struct App {
    schematic_path: String,
    save_path: String,
    file_name: String,
    errors: Vec<AppError>,
    error_map: HashMap<AppError, String>,
    schematic: Option<Schematic>,
}

impl Default for App {
    fn default() -> Self {
        Self {
            schematic_path: String::from("resources/test_schematics/"),
            save_path: String::from("resources/test_obj"),
            file_name: String::from("test"),
            errors: Vec::new(),
            error_map: HashMap::from([
                (AppError::SchematicLoad, String::from("Error loading Schematic"))
            ]),
            schematic: None,
        }
    }
}

impl eframe::App for App {
    fn update(&mut self, ctx: &egui::Context, _frame: &mut eframe::Frame) {
        egui::CentralPanel::default().show(ctx, |ui| {
            ui.heading("Voxel Smith");
            
            ui.label("Schematic: ");
            ui.horizontal(|ui| {
                if ui.button("Open file…").clicked() {
                    if let Some(path) = rfd::FileDialog::new().pick_file() {
                        self.try_load_schematic(path.display().to_string());
                    }
                }
    
                if self.schematic_path.len() > 0 {
                    ui.horizontal(|ui| {
                        ui.label("Picked file:");
                        ui.text_edit_singleline(&mut self.schematic_path);
                    });
                }
            });

            ui.label("Save to: ");
            ui.horizontal(|ui| {
                if ui.button("Open folder…").clicked() {
                    if let Some(path) = rfd::FileDialog::new().pick_folder() {
                        self.save_path = path.display().to_string();
                    }
                }
    
                if self.save_path.len() > 0 {
                    ui.horizontal(|ui| {
                        ui.label("Picked folder:");
                        ui.text_edit_singleline(&mut self.save_path);
                    });
                }
            });

            ui.horizontal(|ui| {
                let file_name_label = ui.label("File Name: ");
                ui.text_edit_singleline(&mut self.file_name)
                    .labelled_by(file_name_label.id);
            });

            ui.horizontal(|ui| {
                if ui.button("Generate").clicked() {
                    self.generate();
                }
    
                if ui.button("Dump JSON").clicked() {
    
                    let _ = _dump_json(&self.schematic_path, &self.file_name);
                }
            });

            for error in &self.errors {
                ui.colored_label(Color32::from_rgb(255, 0, 0), &self.error_map[&error]);
            }

        });
    }
}

impl App {
    fn try_load_schematic(&mut self, file_path: String){
        let schematic = load_schematic(&file_path);

        match schematic {
            Ok(schematic) => {
                self.schematic = Some(schematic);
                self.schematic_path = file_path;

                self.errors.retain(|x| x != &AppError::SchematicLoad)
            }
            Err(_err) => {

                self.schematic_path = "".to_owned();
                self.schematic = None;

                if !self.errors.contains(&AppError::SchematicLoad) {
                    self.errors.push(AppError::SchematicLoad);
                }
            }
        }
    }

    fn generate(&self) {
        if let Some(schematic) = &self.schematic {
            let obj = generate_obj_string(&schematic, &self.file_name);
            let mut file = File::create("".to_owned() + &self.save_path + "/" + &self.file_name + ".obj").unwrap();
            write!(&mut file, "{}\n", obj).unwrap();
        }
    }
}