use crate::schematic;
use schematic::Schematic;

pub fn generate_obj_string(schematic: &Schematic) -> String {

    let mut obj_string: String = String::new();
    let mut v_count: usize = 1;
    let mut f_string: String = String::new();

    let air_value = if schematic.palette.contains_key("minecraft:air") {
        schematic.palette.get("minecraft:air").unwrap().clone()
    }
    else {
        100000
    };

    // normals
    obj_string.push_str(&format!("vn {} {} {}\n", -1, 0, 0));    // 1
    obj_string.push_str(&format!("vn {} {} {}\n", 1, 0, 0));     // 2
    obj_string.push_str(&format!("vn {} {} {}\n", 0, 0, -1));    // 3
    obj_string.push_str(&format!("vn {} {} {}\n", 0, 0, 1));     // 4
    obj_string.push_str(&format!("vn {} {} {}\n", 0, -1, 0));    // 5
    obj_string.push_str(&format!("vn {} {} {}\n", 0, 1, 0));     // 6

    for y in 0..schematic.height {
        for z in 0..schematic.width {
            for x in 0..schematic.length {
                let index = ((y * schematic.length + z) * schematic.width + x) as usize;

                if index >= schematic.block_data.len() { continue; }

                let cube_value = schematic.block_data[index] as i32;

                let width = schematic.width as usize;
                let length = schematic.length as usize;

                if cube_value == air_value { continue; }

                fn check_index(index: usize, schematic: &Schematic, air_value: i32) -> bool {
                    if index >= schematic.block_data.len() { return true; }
                    if schematic.block_data[index] as i32 != air_value { return true; } 

                    return false;
                }

                // Check if adjacent cubes obstruct the face
                let left_obstructed = x > 0 && check_index(index - 1, schematic, air_value);
                let right_obstructed = x < schematic.length - 1 && check_index(index + 1, schematic, air_value);
                let front_obstructed = z > 0 && check_index(index - width, schematic, air_value);
                let back_obstructed = z < schematic.width - 1 && check_index(index + width, schematic, air_value);
                let bottom_obstructed = y > 0 && check_index(index - length * width, schematic, air_value);
                let top_obstructed = y < schematic.length - 1 && check_index(index + length * width, schematic, air_value);

                //obj_string.push_str(&format!("o cube_{}_{}_{}\n", x, y, z));

                if !left_obstructed {
                    obj_string.push_str(&format!("v {} {} {}\n", x, y, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x, y, z + 1));
                    obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z + 1));
                    obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z));

                    f_string.push_str(&format!(
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

                    f_string.push_str(&format!(
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

                    f_string.push_str(&format!(
                        "f {}//{} {}//{} {}//{} {}//{}\n", 
                        v_count, 3, v_count + 1, 3, v_count + 2, 3, v_count + 3, 3)
                    );
                    v_count += 4;
                }

                if !front_obstructed {
                    obj_string.push_str(&format!("v {} {} {}\n", x, y, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y + 1, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y, z));

                    f_string.push_str(&format!(
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

                    f_string.push_str(&format!(
                        "f {}//{} {}//{} {}//{} {}//{}\n", 
                        v_count, 5, v_count + 1, 5, v_count + 2, 5, v_count + 3, 5)
                    );
                    v_count += 4;
                }

                if !top_obstructed {
                    obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z + 1));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y + 1, z + 1));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y + 1, z));

                    f_string.push_str(&format!(
                        "f {}//{} {}//{} {}//{} {}//{}\n", 
                        v_count, 6, v_count + 1, 6, v_count + 2, 6, v_count + 3, 6)
                    );
                    v_count += 4;
                }
            }
        }
    }

    obj_string.push_str(&f_string);

    obj_string
}
