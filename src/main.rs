mod schematic;
mod mesh_generation;

use std::{fs::File, io::{self, Write}};

use obj::Obj;
use schematic::load_schematic;
use mesh_generation::generate_mesh;

const PATH: &str = "resources/StoneRock1.schem";

fn main() {
    let _ = test_export_to_obj_cube("cube.obj");

    // let schematic = load_schematic(PATH);

    // match schematic {
    //     Ok(schematic) => {
    //         // println!("{}", schematic.block_data[0]);
    //         // println!("{}", schematic.block_data[5]);
    //         // println!("{:?}", schematic);

    //         let obj = generate_mesh("TestMesh", &schematic);
    //         let _ = export_to_obj_file(&obj, "out.obj");
    //     }
    //     Err(err) => {
    //         eprintln!("Error decoding schematic: {:?}", err);
    //     }
    // }

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

fn export_to_obj_file(obj: &Obj, filename: &str) -> io::Result<()> {
    let mut file = File::create(filename)?;

    write!(
        &mut file,
        "o Object\n",
    )?;

    for vertex in &obj.vertices {
        write!(
            &mut file,
            "v {} {} {} vn {} {} {}\n",
            vertex.position[0], vertex.position[1], vertex.position[2],
            vertex.normal[0], vertex.normal[1], vertex.normal[2]
        )?;
    }

    for i in (0..obj.indices.len()).step_by(3) {
        write!(
            &mut file,
            "f {}//{} {}//{} {}//{} \n",
            obj.indices[i],
            obj.indices[i],
            obj.indices[i + 1],
            obj.indices[i + 1],
            obj.indices[i + 2],
            obj.indices[i + 2],
        )?;
    }

    Ok(())
}

// fn export_to_obj_file(obj: &Obj, filename: &str) -> io::Result<()> {
//     let mut file = File::create(filename)?;

//     for vertex in &obj.vertices {
//         write!(
//             &mut file,
//             "v {} {} {}\n",
//             vertex.position[0], vertex.position[1], vertex.position[2]
//         )?;
//     }

//     for vertex in &obj.vertices {
//         write!(
//             &mut file,
//             "vn {} {} {}\n",
//             vertex.normal[0], vertex.normal[1], vertex.normal[2]
//         )?;
//     }

//     for i in (0..obj.indices.len()).step_by(3) {
//         write!(
//             &mut file,
//             "f {} {} {} \n",
//             obj.indices[i],
//             obj.indices[i + 1],
//             obj.indices[i + 2],
//         )?;
//     }

//     Ok(())
// }

