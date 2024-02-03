// hide console window on Windows in release
#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")] use std::{fs::File, io::Write};

use eframe::egui;

use crate::{obj_generation, schematic::{self, _dump_json}};
use schematic::load_schematic;
use obj_generation::generate_obj_string;

pub struct App {
    schematic_path: String,
    file_path: String,
    file_name: String,
}

impl Default for App {
    fn default() -> Self {
        Self {
            schematic_path: String::from("resources/test_schematics/"),
            file_path: String::from("resources/test_obj/"),
            file_name: String::from("test"),
        }
    }
}

impl eframe::App for App {
    fn update(&mut self, ctx: &egui::Context, _frame: &mut eframe::Frame) {
        egui::CentralPanel::default().show(ctx, |ui| {
            ui.heading("Voxel Smith");

            ui.horizontal(|ui| {
                let schem_path_label = ui.label("Schematic filepath: ");
                ui.text_edit_singleline(&mut self.schematic_path)
                    .labelled_by(schem_path_label.id);
            });

            ui.horizontal(|ui| {
                let file_path_label = ui.label("Save to: ");
                ui.text_edit_singleline(&mut self.file_path)
                    .labelled_by(file_path_label.id);
            });

            ui.horizontal(|ui| {
                let file_name_label = ui.label("File Name: ");
                ui.text_edit_singleline(&mut self.file_name)
                    .labelled_by(file_name_label.id);
            });

            if ui.button("Generate").clicked() {
                generate(&self.schematic_path, &self.file_path, &self.file_name)
            }

            if ui.button("Dump JSON").clicked() {
                _dump_json(&self.schematic_path, &self.file_name);
            }
        });
    }
}

fn generate(schem_path: &str, file_path: &str, file_name: &str) {
    let schematic = load_schematic(schem_path);

    match schematic {
        Ok(schematic) => {
            let obj = generate_obj_string(&schematic, file_name);
            let mut file = File::create(file_path.to_owned() + file_name + ".obj").unwrap();
            write!(&mut file, "{}\n", obj).unwrap();
        }
        Err(err) => {
            eprintln!("Error decoding schematic: {:?}", err);
        }
    }
}