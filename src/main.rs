mod schematic;
mod obj_generation;
mod app;

use eframe::egui;
use app::App;

fn main() -> Result<(), eframe::Error> {

    eframe::run_native(
        "Voxel Smith",
        eframe::NativeOptions {
            viewport: egui::ViewportBuilder::default()
                .with_inner_size([320.0, 240.0])
                .with_drag_and_drop(true),
            ..Default::default()
        },
        Box::new(|cc| {
            // This gives us image support:
            egui_extras::install_image_loaders(&cc.egui_ctx);
            Box::<App>::default()
        }),
    )
}