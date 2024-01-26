use obj::Obj;
use obj::Vertex;
use schematic::Schematic;

use crate::schematic;

pub fn generate_mesh(name: &str, schematic: &Schematic) -> Obj {
    let mut vertices: Vec<Vertex> = Vec::new();
    let mut indices: Vec<u16> = Vec::new();

    for x in 0..schematic.width {
        for y in 0..schematic.height {
            for z in 0..schematic.length {
                let index: usize = ((y * schematic.length + z) * schematic.width + x) as usize;
                let value: i8 = schematic.block_data[index];

                if value != 0 {
                    let xf = x as f32;
                    let yf = y as f32;
                    let zf = z as f32;
                    
                    let x_positive_side_value: usize = ((y * schematic.length + z) * schematic.width + (x + 1)) as usize;
                    

                    if x_positive_side_value < schematic.block_data.len() && 
                        schematic.block_data[x_positive_side_value] != 0 {
                        let side_vertices: Vec<Vertex> = vec![
                            Vertex { 
                                position: [xf + 1.0, yf, zf], 
                                normal: [1.0, 0.0, 0.0] 
                            },
                            Vertex { 
                                position: [xf + 1.0, yf, zf + 1.0], 
                                normal: [1.0, 0.0, 0.0] 
                            },
                            Vertex { 
                                position: [xf + 1.0, yf + 1.0, zf + 1.0], 
                                normal: [1.0, 0.0, 0.0] 
                            },
                            Vertex { 
                                position: [xf + 1.0, yf + 1.0, zf], 
                                normal: [1.0, 0.0, 0.0] 
                            },
                        ];

                        vertices.extend(side_vertices.clone());

                        let side_indices: Vec<u16> = vec![
                            0, 1, 2, 2, 3, 0
                        ];

                        indices.extend(
                            side_indices.iter().map(|&i| i + (vertices.len() as u16 - side_vertices.len() as u16)),
                        );
                    }


                    let x_negative_side_value: usize = ((y * schematic.length + z) * schematic.width + (x - 1)) as usize;
                    if x_negative_side_value < schematic.block_data.len() &&  
                        schematic.block_data[x_negative_side_value] != 0 {
                        let side_vertices: Vec<Vertex> = vec![
                            Vertex { 
                                position: [xf, yf, zf], 
                                normal: [-1.0, 0.0, 0.0] 
                            },
                            Vertex { 
                                position: [xf, yf, zf + 1.0], 
                                normal: [-1.0, 0.0, 0.0] 
                            },
                            Vertex { 
                                position: [xf, yf + 1.0, zf + 1.0], 
                                normal: [-1.0, 0.0, 0.0] 
                            },
                            Vertex { 
                                position: [xf, yf + 1.0, zf], 
                                normal: [-1.0, 0.0, 0.0] 
                            },
                        ];

                        vertices.extend(side_vertices.clone());

                        let side_indices: Vec<u16> = vec![
                            0, 1, 2, 2, 3, 0
                        ];

                        indices.extend(
                            side_indices.iter().map(|&i| i + (vertices.len() as u16 - side_vertices.len() as u16)),
                        );
                    }
                    

                    let y_positive_side_value: usize = (((y + 1) * schematic.length + z) * schematic.width + x) as usize;
                    if y_positive_side_value < schematic.block_data.len() &&  
                        schematic.block_data[y_positive_side_value] != 0 {
                        let side_vertices: Vec<Vertex> = vec![
                            Vertex { 
                                position: [xf, yf + 1.0, zf], 
                                normal: [0.0, 1.0, 0.0] 
                            },
                            Vertex { 
                                position: [xf, yf + 1.0, zf + 1.0], 
                                normal: [0.0, 1.0, 0.0] 
                            },
                            Vertex { 
                                position: [xf + 1.0, yf + 1.0, zf + 1.0], 
                                normal: [0.0, 1.0, 0.0] 
                            },
                            Vertex { 
                                position: [xf + 1.0, yf + 1.0, zf], 
                                normal: [0.0, 1.0, 0.0] 
                            },
                        ];

                        vertices.extend(side_vertices.clone());

                        let side_indices: Vec<u16> = vec![
                            0, 1, 2, 2, 3, 0
                        ];

                        indices.extend(
                            side_indices.iter().map(|&i| i + (vertices.len() as u16 - side_vertices.len() as u16)),
                        );
                    }


                    let y_negative_side_value: usize = ((y * schematic.length + z) * schematic.width + (x - 1)) as usize;
                    if y_negative_side_value < schematic.block_data.len() &&  
                        schematic.block_data[y_negative_side_value] != 0 {
                        let side_vertices: Vec<Vertex> = vec![
                            Vertex { 
                                position: [xf, yf, zf], 
                                normal: [0.0, -1.0, 0.0] 
                            },
                            Vertex { 
                                position: [xf + 1.0, yf, zf], 
                                normal: [0.0, -1.0, 0.0] 
                            },
                            Vertex { 
                                position: [xf + 1.0, yf, zf + 1.0], 
                                normal: [0.0, -1.0, 0.0] 
                            },
                            Vertex { 
                                position: [xf, yf, zf + 1.0], 
                                normal: [0.0, -1.0, 0.0] 
                            },
                        ];

                        vertices.extend(side_vertices.clone());

                        let side_indices: Vec<u16> = vec![
                            0, 1, 2, 2, 3, 0
                        ];

                        indices.extend(
                            side_indices.iter().map(|&i| i + (vertices.len() as u16 - side_vertices.len() as u16)),
                        );
                    }

                    let z_positive_side_value: usize = ((y * schematic.length + (z + 1)) * schematic.width + x) as usize;
                    if z_positive_side_value < schematic.block_data.len() &&  
                        schematic.block_data[z_positive_side_value] != 0 {
                        let side_vertices: Vec<Vertex> = vec![
                            Vertex {
                                position: [xf, yf, zf + 1.0],
                                normal: [0.0, 0.0, 1.0],
                            },
                            Vertex {
                                position: [xf + 1.0, yf, zf + 1.0],
                                normal: [0.0, 0.0, 1.0],
                            },
                            Vertex {
                                position: [xf + 1.0, yf + 1.0, zf + 1.0],
                                normal: [0.0, 0.0, 1.0],
                            },
                            Vertex {
                                position: [xf, yf + 1.0, zf + 1.0],
                                normal: [0.0, 0.0, 1.0],
                            },
                        ];

                        vertices.extend(side_vertices.clone());

                        let side_indices: Vec<u16> = vec![
                            0, 1, 2, 2, 3, 0
                        ];

                        indices.extend(
                            side_indices.iter().map(|&i| i + (vertices.len() as u16 - side_vertices.len() as u16)),
                        );
                    }


                    let z_negative_side_value: usize = ((y * schematic.length + (z - 1)) * schematic.width + x) as usize;
                    if z_negative_side_value < schematic.block_data.len() && 
                        schematic.block_data[z_negative_side_value] != 0 {
                        let side_vertices: Vec<Vertex> = vec![
                            Vertex {
                                position: [xf, yf, zf],
                                normal: [0.0, 0.0, -1.0],
                            },
                            Vertex {
                                position: [xf, yf + 1.0, zf],
                                normal: [0.0, 0.0, -1.0],
                            },
                            Vertex {
                                position: [xf + 1.0, yf + 1.0, zf],
                                normal: [0.0, 0.0, -1.0],
                            },
                            Vertex {
                                position: [xf + 1.0, yf, zf],
                                normal: [0.0, 0.0, -1.0],
                            },
                        ];

                        vertices.extend(side_vertices.clone());

                        let side_indices: Vec<u16> = vec![
                            0, 1, 2, 2, 3, 0
                        ];

                        indices.extend(
                            side_indices.iter().map(|&i| i + (vertices.len() as u16 - side_vertices.len() as u16)),
                        );
                    }

                    
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


// for x_diff in -1..1  {
//     for y_diff in -1..1 {
//         for z_diff in -1..1 {
//             if  i32::abs(x_diff) == i32::abs(y_diff) || 
//                 i32::abs(y_diff) == i32::abs(z_diff) || 
//                 i32::abs(x_diff) == i32::abs(z_diff) 
//             {
//                 continue;
//             }
            
//             let neighbour_index: usize = (((y + y_diff) * schematic.length + (z + z_diff)) * schematic.width + (x + x_diff)) as usize;
//             let neighbour_value: i8 = schematic.block_data[neighbour_index];
            
//             if neighbour_value != 0 {
//                 continue;
//             }

//             let cube_vertices: Vec<Vertex> = vec![
//                 Vertex { 
//                     position: [xf, yf, zf], 
//                     normal: [x_diff as f32, y_diff as f32, z_diff as f32] 
//                 },
//             ];

//             vertices.extend(cube_vertices.clone());

//             let cube_indices: Vec<u16> = vec![
//                 0, 1, 2, 2, 3, 0, 4, 5, 6, 6, 7, 4, 0, 4, 7, 7, 3, 0, 1, 5, 6, 6, 2, 1,
//             ];

//             indices.extend(
//                 cube_indices.iter().map(|&i| i + (vertices.len() as u16 - cube_vertices.len() as u16)),
//             );
//         }
//     }
// }
