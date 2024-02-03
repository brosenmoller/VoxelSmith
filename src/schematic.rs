use fastnbt::{error::Result, from_bytes, ByteArray, Value};
use serde::Deserialize;
use flate2::read::GzDecoder;
use std::{collections::HashMap, fs::File, io::{Read, Write}};

#[derive(Deserialize, Debug)]
#[serde(rename_all = "PascalCase")]
pub struct Schematic {
    pub width: i32,
    pub height: i32,
    pub length: i32,
    pub block_data: ByteArray,
    pub palette: HashMap<String, i32>,
}

pub fn load_schematic(path: &str) -> Result<Schematic>
{
    let file = File::open(path).unwrap();

    let mut decoder = GzDecoder::new(file);
    let mut data = vec![];
    decoder.read_to_end(&mut data).unwrap();

    return from_bytes(data.as_slice());
}

pub fn _dump_json(path: &str, file_name: &str) {
    let file = File::open(path).unwrap();

    let mut decoder = GzDecoder::new(file);
    let mut data = vec![];
    decoder.read_to_end(&mut data).unwrap();

    let schematic: Result<Value> = from_bytes(data.as_slice());
    let mut file = File::create("resources/schematic_json/".to_owned() + file_name + ".json").unwrap();
    write!(&mut file, "{:?}", schematic).unwrap();
}
