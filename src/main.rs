mod schematic;

use schematic::load_schematic;
use obj::Obj;
use obj::Vertex;
use schematic::Schematic;

const PATH: &str = "resources/StoneRock1.schem";

fn main() {
    let schematic = load_schematic(PATH);

    match schematic {
        Ok(schematic) => {
            println!("{}", schematic.block_data[0]);
            println!("{}", schematic.block_data[5]);
            println!("{:?}", schematic);

            let obj = generate_mesh("TestMesh", &schematic);
        }
        Err(err) => {
            eprintln!("Error decoding schematic: {:?}", err);
        }
    }

    // EVERYTHING
    //let schematic: Result<Value> = from_bytes(data.as_slice());
}

fn generate_mesh(name: &str, schematic: &Schematic) -> Obj {
    let mut vertices: Vec<Vertex> = Vec::new();
    let mut indices: Vec<u16> = Vec::new();

    for y in 0..schematic.height {
        for z in 0..schematic.length {
            for x in 0..schematic.width {
                let index: usize = ((y * schematic.length + z) * schematic.width + x) as usize;
                let value: i8 = schematic.block_data[index];

                if value != 0 {
                    let x = x as f32;
                    let y = y as f32;
                    let z = z as f32;

                    let cube_vertices: Vec<Vertex> = vec![
                        Vertex { position: [x, y, z], normal: [0.0, 0.0, 0.0] },
                        Vertex { position: [x + 1.0, y, z], normal: [0.0, 0.0, 0.0] },
                        Vertex { position: [x + 1.0, y + 1.0, z], normal: [0.0, 0.0, 0.0] },
                        Vertex { position: [x, y + 1.0, z], normal: [0.0, 0.0, 0.0] },
                        Vertex { position: [x, y, z + 1.0], normal: [0.0, 0.0, 0.0] },
                        Vertex { position: [x + 1.0, y, z + 1.0], normal: [0.0, 0.0, 0.0] },
                        Vertex { position: [x + 1.0, y + 1.0, z + 1.0], normal: [0.0, 0.0, 0.0] },
                        Vertex { position: [x, y + 1.0, z + 1.0], normal: [0.0, 0.0, 0.0] },
                    ];

                    vertices.extend(cube_vertices.iter().map(|v| Vertex::from(v.clone())));

                    let cube_indices: Vec<u32> = vec![
                        0, 1, 2, 2, 3, 0, 4, 5, 6, 6, 7, 4, 0, 4, 7, 7, 3, 0, 1, 5, 6, 6, 2, 1,
                    ];

                    indices.extend(
                        cube_indices.iter().map(|&i| i + (vertices.len() as u32 - cube_vertices.len() as u32)),
                    );
                }
            }
        }
    }

    Obj { 
        name: Some(String::from(name)), 
        vertices, 
        indices,
    }
}


