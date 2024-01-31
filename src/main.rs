mod schematic;
mod mesh_generation;
mod obj_generation;

use std::{fs::File, io::{self, Write}};

use obj::Obj;
use schematic::{load_schematic, Schematic};
use mesh_generation::generate_mesh;
use obj_generation::generate_obj;

const PATH: &str = "resources/DarkOakTree1.schem";
const FILE_NAME: &str = "tree";

fn main() {
    let schematic = load_schematic(PATH);

    match schematic {
        Ok(schematic) => {
            // println!("{}\n", schematic.block_data[0]);
            // println!("{}\n", schematic.block_data[5]);
            // println!("{:?}", schematic);

            let obj = generate_obj_file(&schematic);
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


fn test_export_to_obj_cube(filename: &str) -> io::Result<()> {
    let mut file = File::create(filename)?;

    write!(
        &mut file,
        "# Cube.obj\n

        # Vertices\n
        v -1.0 -1.0 -1.0\n
        v -1.0 -1.0  1.0\n
        v -1.0  1.0 -1.0\n
        v -1.0  1.0  1.0\n
        v  1.0 -1.0 -1.0\n
        v  1.0 -1.0  1.0\n
        v  1.0  1.0 -1.0\n
        v  1.0  1.0  1.0\n
        
        # Faces\n
        f 1 2 4 3\n
        f 5 6 8 7\n
        f 1 2 6 5\n
        f 3 4 8 7\n
        f 1 3 7 5\n
        f 2 4 8 6\n",
    )?;

    Ok(())
}

fn generate_obj_file(schematic: &Schematic) -> String {

    let mut obj_string: String = String::new();
    let mut index_count: usize = 0;

    for y in 0..schematic.height {
        for z in 0..schematic.width {
            for x in 0..schematic.length {
                let index = ((y * schematic.length + z) * schematic.width + x) as usize;

                if index >= schematic.block_data.len() { continue; }

                let cube_value = schematic.block_data[index];

                let width = schematic.width as usize;
                let height = schematic.height as usize;
                let length = schematic.length as usize;

                if cube_value == 0 { continue; }

                // Check if adjacent cubes obstruct the face
                let left_obstructed = x > 0 && schematic.block_data[index - 1] > 0;
                let right_obstructed = x < schematic.length - 1 && schematic.block_data[index + 1] > 0;
                let front_obstructed = z > 0 && schematic.block_data[index - width] > 0;
                let back_obstructed = z < schematic.width - 1 && schematic.block_data[index + width] > 0;
                let bottom_obstructed = y > 0 && schematic.block_data[index - length * width] > 0;
                let top_obstructed = y < schematic.length - 1 && schematic.block_data[index + length * width] > 0;

                obj_string.push_str(&format!("o cube_{}_{}_{}\n", x, y, z));

                // Only write vertices and faces if the corresponding face is not obstructed
                obj_string.push_str(&format!("v {} {} {}\n", x, y, z));
                obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z));
                obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z + 1));
                obj_string.push_str(&format!("v {} {} {}\n", x, y, z + 1));

                obj_string.push_str(&format!("v {} {} {}\n", x + 1, y, z));
                obj_string.push_str(&format!("v {} {} {}\n", x + 1, y + 1, z));
                obj_string.push_str(&format!("v {} {} {}\n", x + 1, y + 1, z + 1));
                obj_string.push_str(&format!("v {} {} {}\n", x + 1, y, z + 1));

                obj_string.push_str(&format!("v {} {} {}\n", x, y, z));
                obj_string.push_str(&format!("v {} {} {}\n", x + 1, y, z));
                obj_string.push_str(&format!("v {} {} {}\n", x + 1, y + 1, z));
                obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z));

                obj_string.push_str(&format!("v {} {} {}\n", x, y, z + 1));
                obj_string.push_str(&format!("v {} {} {}\n", x + 1, y, z + 1));
                obj_string.push_str(&format!("v {} {} {}\n", x + 1, y + 1, z + 1));
                obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z + 1));

                obj_string.push_str(&format!("v {} {} {}\n", x, y, z));
                obj_string.push_str(&format!("v {} {} {}\n", x + 1, y, z));
                obj_string.push_str(&format!("v {} {} {}\n", x + 1, y, z + 1));
                obj_string.push_str(&format!("v {} {} {}\n", x, y, z + 1));

                obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z));
                obj_string.push_str(&format!("v {} {} {}\n", x + 1, y + 1, z));
                obj_string.push_str(&format!("v {} {} {}\n", x + 1, y + 1, z + 1));
                obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z + 1));

                // Write the corresponding faces
                if !left_obstructed {
                    obj_string.push_str(&format!("f {} {} {} {}\n", index_count + 1, index_count + 2, index_count + 3, index_count + 4));
                }
                if !right_obstructed {
                    obj_string.push_str(&format!("f {} {} {} {}\n", index_count + 5, index_count + 6, index_count + 7, index_count + 8));
                }
                if !front_obstructed {
                    obj_string.push_str(&format!("f {} {} {} {}\n", index_count + 9, index_count + 10, index_count + 11, index_count + 12));
                }
                if !back_obstructed {
                    obj_string.push_str(&format!("f {} {} {} {}\n", index_count + 13, index_count + 14, index_count + 15, index_count + 16));
                }
                if !bottom_obstructed {
                    obj_string.push_str(&format!("f {} {} {} {}\n", index_count + 17, index_count + 18, index_count + 19, index_count + 20));
                }
                if !top_obstructed {
                    obj_string.push_str(&format!("f {} {} {} {}\n", index_count + 21, index_count + 22, index_count + 23, index_count + 24));
                }

                index_count += 24;
                
            }
        }
    }

    obj_string
}
