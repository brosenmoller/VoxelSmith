use fastnbt::{from_bytes, ByteArray, error::{Result, Error}};
use serde::Deserialize;
use flate2::read::GzDecoder;
use std::io::Read;

#[derive(Deserialize, Debug)]
#[serde(rename_all = "PascalCase")]
pub struct Schematic {
    pub width: i32,
    pub height: i32,
    pub length: i32,
    pub block_data: ByteArray,
}

pub fn load_schematic(path: &str) -> Result<Schematic>
{
    let file = std::fs::File::open(path).unwrap();

    let mut decoder = GzDecoder::new(file);
    let mut data = vec![];
    decoder.read_to_end(&mut data).unwrap();

    return from_bytes(data.as_slice());
} 
