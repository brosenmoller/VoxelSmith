use crate::schematic;
use schematic::Schematic;


pub fn generate_obj(schematic: &Schematic) -> String {
    let mut obj_string: String = String::new();

    let mut index_count: usize = 0;

    for x in 0..schematic.width {
        for y in 0..schematic.height {
            for z in 0..schematic.length {
                let index: usize = ((y * schematic.length + z) * schematic.width + x) as usize;
                let value: i8 = schematic.block_data[index];

                if value == 0 { continue; }

                let xf = x as f32;
                let yf = y as f32;
                let zf = z as f32;

                let width = schematic.width as usize;
                let height = schematic.height as usize;
                let length = schematic.length as usize;
                
                obj_string.push_str(&format!("o cube_{}_{}_{}\n", x, y, z));

                obj_string.push_str(&format!("v {} {} {}\n", xf, yf, zf));
                obj_string.push_str(&format!("v {} {} {}\n", xf + 1.0, yf, zf));
                obj_string.push_str(&format!("v {} {} {}\n", xf + 1.0, yf, zf + 1.0));
                obj_string.push_str(&format!("v {} {} {}\n", xf, yf, zf + 1.0));
                obj_string.push_str(&format!("v {} {} {}\n", xf, yf + 1.0, zf));
                obj_string.push_str(&format!("v {} {} {}\n", xf + 1.0, yf + 1.0, zf));
                obj_string.push_str(&format!("v {} {} {}\n", xf + 1.0, yf + 1.0, zf + 1.0));
                obj_string.push_str(&format!("v {} {} {}\n", xf, yf + 1.0, zf + 1.0));

                // add_face(&mut obj_string, index_count, 1, 4, 3, 2, 
                //     x > 0 && schematic.block_data[index - 1] == 0
                // );
                // add_face(&mut obj_string, index_count, 2, 6, 7, 3, 
                //     x < schematic.width - 1 && schematic.block_data[index + 1] == 0
                // );
                // add_face(&mut obj_string, index_count, 4, 3, 7, 8, 
                //     z < schematic.length - 1 && schematic.block_data[index + width] == 0
                // );
                // add_face(&mut obj_string, index_count, 5, 1, 4, 8, 
                //     z > 0 && schematic.block_data[index - width] == 0
                // );
                // add_face(&mut obj_string, index_count, 5, 6, 2, 1, 
                //     y > 0 && schematic.block_data[index - width * length] == 0
                // );
                // add_face(&mut obj_string, index_count, 8, 7, 6, 5, 
                //     y < schematic.height - 1 && schematic.block_data[index + width * length] == 0
                // );

                add_face(&mut obj_string, index_count, 1, 4, 3, 2, 
                    x == 0 || schematic.block_data[index - 1] == 0
                );
                add_face(&mut obj_string, index_count, 2, 6, 7, 3, 
                    x == schematic.width - 1 || schematic.block_data[index + 1] == 0
                );
                add_face(&mut obj_string, index_count, 4, 3, 7, 8, 
                    z == schematic.length - 1 || schematic.block_data[index + width] == 0
                );
                add_face(&mut obj_string, index_count, 5, 1, 4, 8, 
                    z == 0 || schematic.block_data[index - width] == 0
                );
                add_face(&mut obj_string, index_count, 5, 6, 2, 1, 
                    y == 0 || schematic.block_data[index - width * length] == 0
                );
                add_face(&mut obj_string, index_count, 8, 7, 6, 5, 
                    y == schematic.height - 1 || schematic.block_data[index + width * length] == 0
                );
                
                // add_face(&mut obj_string, index_count, 1, 4, 3, 2, 
                //     x == 0 || schematic.block_data[index - 1] == 0
                // );
                // add_face(&mut obj_string, index_count, 2, 6, 7, 3, 
                //     x == schematic.width - 1 || schematic.block_data[index + 1] == 0
                // );
                // add_face(&mut obj_string, index_count, 4, 3, 7, 8, 
                //     z == schematic.length - 1 || schematic.block_data[index + width] == 0
                // );
                // add_face(&mut obj_string, index_count, 5, 1, 4, 8, 
                //     z == 0 || schematic.block_data[index - width] == 0
                // );
                // add_face(&mut obj_string, index_count, 5, 6, 2, 1, 
                //     y == 0 || schematic.block_data[((y as usize + 1) * length + z as usize) * width + x as usize] == 0
                // );
                // add_face(&mut obj_string, index_count, 8, 7, 6, 5, 
                //     y == schematic.height - 1 || schematic.block_data[((y as usize - 1) * length + z as usize) * width + x as usize] == 0
                // );

                index_count += 8;
                
                // Generate face if block is on the outside or has no block next to it
                // {
                //     let side_index: usize = ((y * schematic.length + z) * schematic.width + (x + 1)) as usize;

                //     if side_index > schematic.block_data.len() || x == schematic.width - 1 || schematic.block_data[side_index] != 0 {
                
                //         obj_string.push_str(&format!("f {} {} {} {}\n", 
                //             2 + index_count, 6 + index_count, 7 + index_count, 3 + index_count)
                //         );

                //     }
                // }

                // {
                //     let side_index: usize = ((y * schematic.length + z) * schematic.width + (x - 1)) as usize;

                //     if  side_index > schematic.block_data.len() || x == 0 || schematic.block_data[side_index] != 0 {
                
                //         obj_string.push_str(&format!("f {} {} {} {}\n", 
                //             1 + index_count, 4 + index_count, 8 + index_count, 5 + index_count)
                //         );
                        
                //     }
                // }

                // {
                //     let side_index: usize = (((y + 1) * schematic.length + z) * schematic.width + x) as usize;

                //     if  side_index > schematic.block_data.len() || y == schematic.height - 1 || schematic.block_data[side_index] != 0 {
                
                //         obj_string.push_str(&format!("f {} {} {} {}\n", 
                //             5 + index_count, 8 + index_count, 7 + index_count, 6 + index_count)
                //         );
                        
                //     }
                // }

                // {
                //     let side_index: usize = (((y - 1) * schematic.length + z) * schematic.width + x) as usize;

                //     if side_index > schematic.block_data.len() || y == 0 || schematic.block_data[side_index] != 0 {
                    
                //         obj_string.push_str(&format!("f {} {} {} {}\n", 
                //             1 + index_count, 2 + index_count, 3 + index_count, 4 + index_count)
                //         );
                        
                //     }
                // }
                
                // {
                //     let side_index: usize = ((y * schematic.length + (z + 1)) * schematic.width + x) as usize;

                //     if  side_index > schematic.block_data.len() || z == schematic.length - 1 || schematic.block_data[side_index] != 0 {
                
                //         obj_string.push_str(&format!("f {} {} {} {}\n", 
                //             4 + index_count, 3 + index_count, 7 + index_count, 8 + index_count)
                //         );
                        
                //     }
                // }

                // {
                //     let side_index: usize = ((y * schematic.length + (z - 1)) * schematic.width + x) as usize;

                //     if side_index > schematic.block_data.len() || z == 0 || schematic.block_data[side_index] != 0 {
                
                //         obj_string.push_str(&format!("f {} {} {} {}\n", 
                //             1 + index_count, 5 + index_count, 6 + index_count, 2 + index_count)
                //         );
                        
                //     }
                // }
                
                // index_count += 8;
                
            }
        }
    }

    obj_string
}

fn add_face(obj_string: &mut String, index_count: usize, i1: usize, i2: usize, i3: usize, i4: usize, not_obstructed: bool) {
    if not_obstructed {
        obj_string.push_str(&format!("f {} {} {} {}\n", i1 + index_count, i2 + index_count, i3 + index_count, i4 + index_count));
    }
}

// pub fn generate_obj(schematic: &Schematic) -> String {
//     let mut obj_string: String = String::new();

//     let mut index_count: usize = 0;

//     for x in 0..schematic.width {
//         for y in 0..schematic.height {
//             for z in 0..schematic.length {
//                 let index: usize = ((y * schematic.length + z) * schematic.width + x) as usize;
//                 let value: i8 = schematic.block_data[index];

//                 if value != 0 {
//                     let xf = x as f32;
//                     let yf = y as f32;
//                     let zf = z as f32;
                    
//                     obj_string.push_str(&format!("o cube_{}_{}_{}\n", x, y, z));

//                     obj_string.push_str(&format!("v {} {} {}\n", xf, yf, zf));
//                     obj_string.push_str(&format!("v {} {} {}\n", xf + 1.0, yf, zf));
//                     obj_string.push_str(&format!("v {} {} {}\n", xf + 1.0, yf, zf + 1.0));
//                     obj_string.push_str(&format!("v {} {} {}\n", xf, yf, zf + 1.0));
//                     obj_string.push_str(&format!("v {} {} {}\n", xf, yf + 1.0, zf));
//                     obj_string.push_str(&format!("v {} {} {}\n", xf + 1.0, yf + 1.0, zf));
//                     obj_string.push_str(&format!("v {} {} {}\n", xf + 1.0, yf + 1.0, zf + 1.0));
//                     obj_string.push_str(&format!("v {} {} {}\n", xf, yf + 1.0, zf + 1.0));

//                     obj_string.push_str(&format!("f {} {} {} {}\n", 1 + index_count, 2 + index_count, 3 + index_count, 4 + index_count));
//                     obj_string.push_str(&format!("f {} {} {} {}\n", 5 + index_count, 8 + index_count, 7 + index_count, 6 + index_count));
//                     obj_string.push_str(&format!("f {} {} {} {}\n", 2 + index_count, 6 + index_count, 7 + index_count, 3 + index_count));
//                     obj_string.push_str(&format!("f {} {} {} {}\n", 1 + index_count, 4 + index_count, 8 + index_count, 5 + index_count));
//                     obj_string.push_str(&format!("f {} {} {} {}\n", 1 + index_count, 5 + index_count, 6 + index_count, 2 + index_count));
//                     obj_string.push_str(&format!("f {} {} {} {}\n", 4 + index_count, 3 + index_count, 7 + index_count, 8 + index_count));
                    
//                     index_count += 8;
//                 }
//             }
//         }
//     }

//     obj_string
// }