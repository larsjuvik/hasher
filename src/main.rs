//! A lightning-fast application for simple verification of hashes

// For CLI-tool 'clap'
use clap::Parser;
use std::str::FromStr;

// For hashing
use sha2::{Sha256, Digest};

// For IO
use std::fs::File;
use std::io::Read;

const BUFFER_SIZE: usize = 1024;

/// Available algorithms for hashing
#[derive(Debug)]
enum Algorithm {
    SHA256,
}
impl FromStr for Algorithm {  // turn CLI-arg into enum
    type Err = String;
    
    fn from_str(s: &str) -> Result<Self, Self::Err> {
        match s {
            "sha256" | "SHA256" => Ok(Algorithm::SHA256),
            _ => Err(format!("Error -> could not parse \"{}\" to enum 'Algorithm'", s)),
        }
    }
}

// Configuration for clap; CLI-tool
/// A lightning-fast application for simple verification of hashes
#[derive(Parser, Debug)]
#[clap(about, version, author)]
struct Arguments {
    /// Which hashing-algorithm to use
    #[clap(short, long, required=false, default_value="SHA256")]
    alg: Algorithm,

    /// Name of the file to hash
    #[clap(short, long, required=true)]
    file: String,
}

fn main() {
    let arguments = Arguments::parse();

    // Opening file (read-only) 
    let mut file = match File::open(arguments.file) {
        Ok(f) => f,
        Err(err) => panic!("Error when opening file: {}", err),
    };

    // Creating new hasher-instance, and a buffer.
    // Each time the buffer is filled up, update the hasher with it.
    let mut hasher = match arguments.alg {
        Algorithm::SHA256 => Sha256::new(),
    };
    let mut buffer: [u8; BUFFER_SIZE] = [0; BUFFER_SIZE];

    // Looping while the file has bytes left to read
    loop {
        // Reads bytes into buffer, returns bytes read
        let num_bytes_read = match file.read(&mut buffer) {
            Ok(n) => n,
            Err(err) => panic!("Error while reading bytes from file: {}", err),
        };

        if num_bytes_read == 0 {
            break
        } else {
            // If at end of file, and read less bytes than buffer
            // => Read last bytes into vector, update hasher and exit loop
            if num_bytes_read < buffer.len() {
                let data: Vec<u8> = buffer[..num_bytes_read].to_vec();
                hasher.update(data);
                break
            } else {
                hasher.update(buffer);
            }
        }
    }

    let result = hasher.finalize();
    println!("0x{:x}", result);
}