mod schematic;
mod obj_generation;

use std::{fs::File, io::Write};

use schematic::load_schematic;
use obj_generation::generate_obj_string;

const PATH: &str = "resources/test_schematics/DarkOakTree1.schem";
const FILE_NAME: &str = "resources/test_obj/tree2";

fn main() {
    let schematic = load_schematic(PATH);

    match schematic {
        Ok(schematic) => {
            let obj = generate_obj_string(&schematic);
            let mut file = File::create(FILE_NAME.to_owned() +".obj").unwrap();
            write!(&mut file, "{}\n", obj).unwrap();
        }
        Err(err) => {
            eprintln!("Error decoding schematic: {:?}", err);
        }
    }

    //let schematic: Result<Value> = from_bytes(data.as_slice());
}