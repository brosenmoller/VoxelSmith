mod schematic;
mod obj_generation;

use std::{fs::File, io::Write};

use schematic::load_schematic;
use obj_generation::generate_obj_string;

const PATH: &str = "resources/DarkOakTree1.schem";
const FILE_NAME: &str = "tree";

fn main() {
    let schematic = load_schematic(PATH);

    match schematic {
        Ok(schematic) => {
            // println!("{}\n", schematic.block_data[0]);
            // println!("{}\n", schematic.block_data[5]);
            // println!("{:?}", schematic);

            let obj = generate_obj_string(&schematic);
            let mut file = File::create(FILE_NAME.to_owned() +".obj").unwrap();
            write!(&mut file, "{}\n", obj).unwrap();

            //let _ = export_to_obj_file(&obj, "out.obj");
        }
        Err(err) => {
            eprintln!("Error decoding schematic: {:?}", err);
        }
    }

    // EVERYTHING
    //let schematic: Result<Value> = from_bytes(data.as_slice());
}