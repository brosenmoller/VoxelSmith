use crate::schematic;
use schematic::Schematic;

pub fn generate_obj_string(schematic: &Schematic, name: &str) -> String {

    let mut obj_string: String = String::new();
    let mut v_count: usize = 1;

    let air_value = match schematic.palette.get("minecraft:air") {
        Some(value) => value.clone(),
        None => 100000,
    };

    obj_string.push_str("# Mesh Exported Using VoxelSmith:\n");
    obj_string.push_str("# https://github.com/brosenmoller/VoxelSmith\n\n");
    obj_string.push_str(&format!("o {}\n\n", name));

    // normals
    obj_string.push_str(&format!("vn {} {} {}\n", -1, 0, 0));    // 1
    obj_string.push_str(&format!("vn {} {} {}\n", 1, 0, 0));     // 2
    obj_string.push_str(&format!("vn {} {} {}\n", 0, 0, -1));    // 3
    obj_string.push_str(&format!("vn {} {} {}\n", 0, 0, 1));     // 4
    obj_string.push_str(&format!("vn {} {} {}\n", 0, -1, 0));    // 5
    obj_string.push_str(&format!("vn {} {} {}\n", 0, 1, 0));     // 6


    let schematic_len = (schematic.height * schematic.length * schematic.width) as usize;
    let mut block_data: Vec<i8> = Vec::with_capacity(schematic_len);

    for i in 0..schematic.block_data.len() {
        let block_value = schematic.block_data[i];

        if block_value >= 0 {
            block_data.push(block_value);
        }
    }

    for y in 0..schematic.height {
        for x in 0..schematic.width {
            for z in 0..schematic.length {
                let index = ((y * schematic.length + z) * schematic.width + x) as usize;

                if index >= block_data.len() { continue; }

                let block_value = block_data[index] as i32;

                if block_value == air_value { continue; }

                fn is_block_obstructed(index: i32, block_data: &Vec<i8>, air_value: i32) -> bool {
                    if index >= 0 && index < block_data.len() as i32 {
                        return block_data[index as usize] as i32 != air_value;
                    }
                    false
                }

                let index_i32: i32 = index as i32;

                // Check if adjacent cubes obstruct the face
                let left_obstructed = x > 0 && is_block_obstructed(index_i32 - 1, 
                    &block_data, air_value);
                let right_obstructed = x < schematic.length - 1 && is_block_obstructed(index_i32 + 1, 
                    &block_data, air_value);
                let front_obstructed = z > 0 && is_block_obstructed(index_i32 - schematic.width, 
                    &block_data, air_value);
                let back_obstructed = z < schematic.width - 1 && is_block_obstructed(index_i32 + schematic.width, 
                    &block_data, air_value);
                let bottom_obstructed = y > 0 && is_block_obstructed(index_i32 - schematic.length * schematic.width, 
                    &block_data, air_value);
                let top_obstructed = y < schematic.length - 1 && is_block_obstructed(index_i32 + schematic.length * schematic.width, 
                    &block_data, air_value);

                //obj_string.push_str(&format!("o cube_{}_{}_{}\n", x, y, z));

                if !left_obstructed {
                    obj_string.push_str(&format!("v {} {} {}\n", x, y, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x, y, z + 1));
                    obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z + 1));
                    obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z));

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
                    obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y + 1, z));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y, z));

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
                    obj_string.push_str(&format!("v {} {} {}\n", x, y + 1, z + 1));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y + 1, z + 1));
                    obj_string.push_str(&format!("v {} {} {}\n", x + 1, y + 1, z));

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
