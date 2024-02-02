use crate::schematic;
use schematic::Schematic;

pub fn generate_obj_string(schematic: &Schematic) -> String {

    let mut obj_string: String = String::new();
    let mut v_count: usize = 1;

    // normals
    obj_string.push_str(&format!("vn {} {} {}\n", -1, 0, 0));    // 1
    obj_string.push_str(&format!("vn {} {} {}\n", 1, 0, 0));     // 2
    obj_string.push_str(&format!("vn {} {} {}\n", 0, 0, -1));    // 3
    obj_string.push_str(&format!("vn {} {} {}\n", 0, 0, 1));     // 4
    obj_string.push_str(&format!("vn {} {} {}\n", 0, -1, 0));     // 5
    obj_string.push_str(&format!("vn {} {} {}\n", 0, 1, 0));     // 6

    // obj_string.push_str("vn -1.000000 -0.000000 -0.000000\n");     // 1
    // obj_string.push_str("vn 1.000000 0.000000 0.000000\n");     // 2
    // obj_string.push_str("vn 0.000000 0.000000 -1.000000\n");     // 3
    // obj_string.push_str("vn -0.000000 0.000000 1.000000\n");     // 4
    // obj_string.push_str("vn 0.000000 -1.000000 0.000000\n");     // 5
    // obj_string.push_str("vn 0.000000 1.000000 0.000000\n");     // 6

    for y in 0..schematic.height {
        for z in 0..schematic.width {
            for x in 0..schematic.length {
                let index = ((y * schematic.length + z) * schematic.width + x) as usize;

                if index >= schematic.block_data.len() { continue; }

                let cube_value = schematic.block_data[index];

                let width = schematic.width as usize;
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

                if !left_obstructed {
                    obj_string.push_str(&format!("v {} {} {}\n", x, y, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z + 1));
                    obj_string.push_str(&format!("v {} {} {}\n", x, y, z + 1));

                    obj_string.push_str(&format!(
                        "f {}//{} {}//{} {}//{} {}//{}\n", 
                        v_count, 1, v_count + 1, 1, v_count + 2, 1, v_count + 3, 1)
                    );
                    v_count += 4;
                }

                if !right_obstructed {
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y + 1, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y + 1, z + 1));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y, z + 1));

                    obj_string.push_str(&format!(
                        "f {}//{} {}//{} {}//{} {}//{}\n", 
                        v_count, 2, v_count + 1, 2, v_count + 2, 2, v_count + 3, 2)
                    );
                    v_count += 4;
                }

                if !back_obstructed {
                    obj_string.push_str(&format!("v {} {} {}\n", x, y, z + 1));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y, z + 1));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y + 1, z + 1));
                    obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z + 1));

                    obj_string.push_str(&format!(
                        "f {}//{} {}//{} {}//{} {}//{}\n", 
                        v_count, 3, v_count + 1, 3, v_count + 2, 3, v_count + 3, 3)
                    );
                    v_count += 4;
                }

                if !front_obstructed {
                    obj_string.push_str(&format!("v {} {} {}\n", x, y, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y + 1, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z));

                    obj_string.push_str(&format!(
                        "f {}//{} {}//{} {}//{} {}//{}\n", 
                        v_count, 4, v_count + 1, 4, v_count + 2, 4, v_count + 3, 4)
                    );
                    v_count += 4;
                }

                if !bottom_obstructed {
                    obj_string.push_str(&format!("v {} {} {}\n", x, y, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y, z + 1));
                    obj_string.push_str(&format!("v {} {} {}\n", x, y, z + 1));

                    obj_string.push_str(&format!(
                        "f {}//{} {}//{} {}//{} {}//{}\n", 
                        v_count, 5, v_count + 1, 5, v_count + 2, 5, v_count + 3, 5)
                    );
                    v_count += 4;
                }

                if !top_obstructed {
                    obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y + 1, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y + 1, z + 1));
                    obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z + 1));

                    obj_string.push_str(&format!(
                        "f {}//{} {}//{} {}//{} {}//{}\n", 
                        v_count, 6, v_count + 1, 6, v_count + 2, 6, v_count + 3, 6)
                    );
                    v_count += 4;
                }
            }
        }
    }

    obj_string
}
