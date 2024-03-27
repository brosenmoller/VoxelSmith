use godot::prelude::*;
use godot::engine::{mesh, Material, Mesh, MeshInstance3D, StandardMaterial3D, SurfaceTool};
use std::collections::HashMap;

struct MeshExtension;

#[gdextension]
unsafe impl ExtensionLibrary for MeshExtension { }

struct VoxelData {
    pub color: Color
}

#[derive(GodotClass)]
struct MeshGenerator {
    voxel_size: f32,
    default_material: Gd<Material>,
    surface_tool: Gd<SurfaceTool>,
    cube_vertices: [Vector3; 8],
}

#[godot_api]
impl MeshGenerator {
    fn init(base: Base<MeshInstance3D>) -> Self {
        godot_print!("Initializing MeshGenerator from Rust");

        let cube_vertices = [
            Vector3::new(0.0, 0.0, 0.0), Vector3::new(1.0, 0.0, 0.0),
            Vector3::new(1.0, 0.0, 1.0), Vector3::new(0.0, 0.0, 1.0),
            Vector3::new(0.0, 1.0, 0.0), Vector3::new(1.0, 1.0, 0.0),
            Vector3::new(1.0, 1.0, 1.0), Vector3::new(0.0, 1.0, 1.0)
        ];
        
        Self {
            voxel_size: 1.0,
            default_material: Material::new_gd(),
            surface_tool: SurfaceTool::new_gd(),
            cube_vertices,
        }
    }

    #[func]
    fn create_mesh(&mut self, voxels: &HashMap<Vector3i, VoxelData>) -> Mesh {
        self.surface_tool.begin(mesh::PrimitiveType::TRIANGLES);
        self.surface_tool.set_material(self.default_material);

        for voxel in voxels.keys() {
            self.create_voxel(voxel, &voxels[voxel].color, voxels);
        }

        self.surface_tool.index();
        return Some(self.surface_tool.commit());
    }

    fn add_vertex(&mut self, position: Vector3) {
        self.surface_tool.add_vertex(position * self.voxel_size);
    }

    fn create_voxel(&mut self, position: &Vector3i, color: &Color, voxels: &HashMap<Vector3i, VoxelData>) {

        let left = !voxels.contains_key(position* + Vector3i::new(-1, 0, 0));
        let right = !voxels.contains_key(position + Vector3i::new(1, 0, 0));
        let bottom = !voxels.contains_key(position + Vector3i::new(0, -1, 0));
        let top = !voxels.contains_key(position + Vector3i::new(0, 1, 0));
        let back = !voxels.contains_key(position + Vector3i::new(0, 0, -1));
        let front = !voxels.contains_key(position + Vector3i::new(0, 0, 1));

        self.surface_tool.set_color(color);

        if left {
            self.surface_tool.set_normal(Vector3::new(-1.0, 0.0, 0.0));
            add_vertex(vertices[0] + vertex_offset);
            self.add_vertex(vertices[7] + vertex_offset);
            self.add_vertex(vertices[3] + vertex_offset);
            self.add_vertex(vertices[0] + vertex_offset);
            self.add_vertex(vertices[4] + vertex_offset);
            self.add_vertex(vertices[7] + vertex_offset);
        }
        if right {
            surface_tool.set_normal(Vector3::new(1.0, 0.0, 0.0));
            self.add_vertex(vertices[2] + vertex_offset);
            self.add_vertex(vertices[5] + vertex_offset);
            self.add_vertex(vertices[1] + vertex_offset);
            self.add_vertex(vertices[2] + vertex_offset);
            self.add_vertex(vertices[6] + vertex_offset);
            self.add_vertex(vertices[5] + vertex_offset);
        }
        if bottom {
            surface_tool.set_normal(Vector3::new(0.0, 1.0, 0.0));
            self.add_vertex(vertices[1] + vertex_offset);
            self.add_vertex(vertices[3] + vertex_offset);
            self.add_vertex(vertices[2] + vertex_offset);
            self.add_vertex(vertices[1] + vertex_offset);
            self.add_vertex(vertices[0] + vertex_offset);
            self.add_vertex(vertices[3] + vertex_offset);
        }
        if top {
            surface_tool.set_normal(Vector3::new(0.0, -1.0, 0.0));
            self.add_vertex(vertices[4] + vertex_offset);
            self.add_vertex(vertices[5] + vertex_offset);
            self.add_vertex(vertices[7] + vertex_offset);
            self.add_vertex(vertices[5] + vertex_offset);
            self.add_vertex(vertices[6] + vertex_offset);
            self.add_vertex(vertices[7] + vertex_offset);
        }
        if back {
            surface_tool.set_normal(Vector3::new(0.0, 0.0, -1.0));
            self.add_vertex(vertices[0] + vertex_offset);
            self.add_vertex(vertices[1] + vertex_offset);
            self.add_vertex(vertices[5] + vertex_offset);
            self.add_vertex(vertices[5] + vertex_offset);
            self.add_vertex(vertices[4] + vertex_offset);
            self.add_vertex(vertices[0] + vertex_offset);
        }
        if front {
            surface_tool.set_normal(Vector3::new(0.0, 0.0, 1.0));
            self.add_vertex(vertices[3] + vertex_offset);
            self.add_vertex(vertices[6] + vertex_offset);
            self.add_vertex(vertices[2] + vertex_offset);
            self.add_vertex(vertices[3] + vertex_offset);
            self.add_vertex(vertices[7] + vertex_offset);
            self.add_vertex(vertices[6] + vertex_offset);
        }
    }
}