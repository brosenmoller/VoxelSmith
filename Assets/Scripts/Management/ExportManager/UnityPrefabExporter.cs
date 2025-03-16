using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using Godot;

public class UnityPrefabExporter : IExporter
{
    private Random random;
    private MeshObjExporter meshExporter;

    public UnityPrefabExporter(MeshObjExporter meshExporter)
    {
        random = new Random();
        this.meshExporter = meshExporter;
    }

    public void Export(ExportSettingsData exportSettings, EditorData.ExportPathData exportPath)
    {
        string name = exportPath.fileName;
        if (exportPath.fileName.Contains('.'))
        {
            name = exportPath.fileName[..exportPath.fileName.IndexOf(".")];
        }
        // Create New Guid in Untiy style without hyphens
        string meshGuid = Guid.NewGuid().ToString("N");

        string gameObjectComponentFileId = GenerateFileId();
        string transformComponentFileId = GenerateFileId();
        string meshFilterComponentFileId = GenerateFileId();
        string meshRendererComponentFileId = GenerateFileId();
        string meshColliderComponentFileId = GenerateFileId();

        // This is the fileID for a mesh with a "default" group name, I couldn't reverse engineer the hashing function so I will just use an existing hash of that mesh name
        string meshFileId = "-2432090755550338912";
        // This is the fileID for a material named "defaultMat" (same reasons)
        string materialFileId = "-3033667219593020291";

        string prefabFileTemplateBeforeChildren = $"%YAML 1.1\r\n%TAG !u! tag:unity3d.com,2011:\r\n--- !u!1 &{gameObjectComponentFileId}\r\nGameObject:\r\n  m_ObjectHideFlags: 0\r\n  m_CorrespondingSourceObject: {{fileID: 0}}\r\n  m_PrefabInstance: {{fileID: 0}}\r\n  m_PrefabAsset: {{fileID: 0}}\r\n  serializedVersion: 6\r\n  m_Component:\r\n  - component: {{fileID: {transformComponentFileId}}}\r\n  - component: {{fileID: {meshFilterComponentFileId}}}\r\n  - component: {{fileID: {meshRendererComponentFileId}}}\r\n  - component: {{fileID: {meshColliderComponentFileId}}}\r\n  m_Layer: 3\r\n  m_Name: {name}\r\n  m_TagString: Untagged\r\n  m_Icon: {{fileID: 0}}\r\n  m_NavMeshLayer: 0\r\n  m_StaticEditorFlags: 0\r\n  m_IsActive: 1\r\n--- !u!4 &{transformComponentFileId}\r\nTransform:\r\n  m_ObjectHideFlags: 0\r\n  m_CorrespondingSourceObject: {{fileID: 0}}\r\n  m_PrefabInstance: {{fileID: 0}}\r\n  m_PrefabAsset: {{fileID: 0}}\r\n  m_GameObject: {{fileID: {gameObjectComponentFileId}}}\r\n  serializedVersion: 2\r\n  m_LocalRotation: {{x: 0, y: 0, z: 0, w: 1}}\r\n  m_LocalPosition: {{x: 0, y: 0, z: 0}}\r\n  m_LocalScale: {{x: 1, y: 1, z: 1}}\r\n  m_ConstrainProportionsScale: 0\r\n  m_Children:\r\n ";
        string prefabFileTemplateAfterChildren = $" m_Father: {{fileID: 0}}\r\n  m_LocalEulerAnglesHint: {{x: 0, y: 0, z: 0}}\r\n--- !u!33 &{meshFilterComponentFileId}\r\nMeshFilter:\r\n  m_ObjectHideFlags: 0\r\n  m_CorrespondingSourceObject: {{fileID: 0}}\r\n  m_PrefabInstance: {{fileID: 0}}\r\n  m_PrefabAsset: {{fileID: 0}}\r\n  m_GameObject: {{fileID: {gameObjectComponentFileId}}}\r\n  m_Mesh: {{fileID: {meshFileId}, guid: {meshGuid}, type: 3}}\r\n--- !u!23 &{meshRendererComponentFileId}\r\nMeshRenderer:\r\n  m_ObjectHideFlags: 0\r\n  m_CorrespondingSourceObject: {{fileID: 0}}\r\n  m_PrefabInstance: {{fileID: 0}}\r\n  m_PrefabAsset: {{fileID: 0}}\r\n  m_GameObject: {{fileID: {gameObjectComponentFileId}}}\r\n  m_Enabled: 1\r\n  m_CastShadows: 1\r\n  m_ReceiveShadows: 1\r\n  m_DynamicOccludee: 1\r\n  m_StaticShadowCaster: 0\r\n  m_MotionVectors: 1\r\n  m_LightProbeUsage: 1\r\n  m_ReflectionProbeUsage: 1\r\n  m_RayTracingMode: 2\r\n  m_RayTraceProcedural: 0\r\n  m_RenderingLayerMask: 1\r\n  m_RendererPriority: 0\r\n  m_Materials:\r\n  - {{fileID: {materialFileId}, guid: {meshGuid}, type: 3}}\r\n  m_StaticBatchInfo:\r\n    firstSubMesh: 0\r\n    subMeshCount: 0\r\n  m_StaticBatchRoot: {{fileID: 0}}\r\n  m_ProbeAnchor: {{fileID: 0}}\r\n  m_LightProbeVolumeOverride: {{fileID: 0}}\r\n  m_ScaleInLightmap: 1\r\n  m_ReceiveGI: 1\r\n  m_PreserveUVs: 0\r\n  m_IgnoreNormalsForChartDetection: 0\r\n  m_ImportantGI: 0\r\n  m_StitchLightmapSeams: 1\r\n  m_SelectedEditorRenderState: 3\r\n  m_MinimumChartSize: 4\r\n  m_AutoUVMaxDistance: 0.5\r\n  m_AutoUVMaxAngle: 89\r\n  m_LightmapParameters: {{fileID: 0}}\r\n  m_SortingLayerID: 0\r\n  m_SortingLayer: 0\r\n  m_SortingOrder: 0\r\n  m_AdditionalVertexStreams: {{fileID: 0}}\r\n--- !u!64 &{meshColliderComponentFileId}\r\nMeshCollider:\r\n  m_ObjectHideFlags: 0\r\n  m_CorrespondingSourceObject: {{fileID: 0}}\r\n  m_PrefabInstance: {{fileID: 0}}\r\n  m_PrefabAsset: {{fileID: 0}}\r\n  m_GameObject: {{fileID: {gameObjectComponentFileId}}}\r\n  m_Material: {{fileID: 0}}\r\n  m_IncludeLayers:\r\n    serializedVersion: 2\r\n    m_Bits: 0\r\n  m_ExcludeLayers:\r\n    serializedVersion: 2\r\n    m_Bits: 0\r\n  m_LayerOverridePriority: 0\r\n  m_IsTrigger: 0\r\n  m_ProvidesContacts: 0\r\n  m_Enabled: 1\r\n  serializedVersion: 5\r\n  m_Convex: 0\r\n  m_CookingOptions: 30\r\n  m_Mesh: {{fileID: {meshFileId}, guid: {meshGuid}, type: 3}}\r\n";

        List<PrefabInstance> instances = new();
        foreach (var paletteItem in GameManager.DataManager.ProjectData.voxelPrefabs)
        {
            if (!GameManager.DataManager.PaletteData.palletePrefabs.ContainsKey(paletteItem.Value)) { continue; }

            VoxelPrefab voxelPrefab = GameManager.DataManager.PaletteData.palletePrefabs[paletteItem.Value];

            instances.Add(new PrefabInstance(
                voxelPrefab.prefabName,
                voxelPrefab.unityPrefabGuid,
                voxelPrefab.unityPrefabTransformFileId,
                GenerateFileId(),
                GenerateFileId(),
                transformComponentFileId,
                paletteItem.Key
            ));
        }

        StringBuilder prefabFile = new();
        prefabFile.Append(prefabFileTemplateBeforeChildren);
        for (int i = 0; i < instances.Count; i++)
        {
            prefabFile.Append(instances[i].GetParentTransformChildString());
        }
        prefabFile.Append(prefabFileTemplateAfterChildren);
        for (int i = 0; i < instances.Count; i++)
        {
            prefabFile.Append(instances[i].GetPrefabInstanceString());
        }

        string meshMetaFileTemplate = $"fileFormatVersion: 2\r\nguid: {meshGuid}\r\nModelImporter:\r\n  serializedVersion: 22200\r\n  internalIDToNameTable: []\r\n  externalObjects: {{}}\r\n  materials:\r\n    materialImportMode: 2\r\n    materialName: 0\r\n    materialSearch: 1\r\n    materialLocation: 1\r\n  animations:\r\n    legacyGenerateAnimations: 4\r\n    bakeSimulation: 0\r\n    resampleCurves: 1\r\n    optimizeGameObjects: 0\r\n    removeConstantScaleCurves: 0\r\n    motionNodeName: \r\n    rigImportErrors: \r\n    rigImportWarnings: \r\n    animationImportErrors: \r\n    animationImportWarnings: \r\n    animationRetargetingWarnings: \r\n    animationDoRetargetingWarnings: 0\r\n    importAnimatedCustomProperties: 0\r\n    importConstraints: 0\r\n    animationCompression: 1\r\n    animationRotationError: 0.5\r\n    animationPositionError: 0.5\r\n    animationScaleError: 0.5\r\n    animationWrapMode: 0\r\n    extraExposedTransformPaths: []\r\n    extraUserProperties: []\r\n    clipAnimations: []\r\n    isReadable: 1\r\n  meshes:\r\n    lODScreenPercentages: []\r\n    globalScale: 1\r\n    meshCompression: 0\r\n    addColliders: 0\r\n    useSRGBMaterialColor: 1\r\n    sortHierarchyByName: 1\r\n    importPhysicalCameras: 1\r\n    importVisibility: 1\r\n    importBlendShapes: 1\r\n    importCameras: 1\r\n    importLights: 1\r\n    nodeNameCollisionStrategy: 1\r\n    fileIdsGeneration: 2\r\n    swapUVChannels: 0\r\n    generateSecondaryUV: 0\r\n    useFileUnits: 1\r\n    keepQuads: 0\r\n    weldVertices: 1\r\n    bakeAxisConversion: 0\r\n    preserveHierarchy: 0\r\n    skinWeightsMode: 0\r\n    maxBonesPerVertex: 4\r\n    minBoneWeight: 0.001\r\n    optimizeBones: 1\r\n    meshOptimizationFlags: -1\r\n    indexFormat: 0\r\n    secondaryUVAngleDistortion: 8\r\n    secondaryUVAreaDistortion: 15.000001\r\n    secondaryUVHardAngle: 88\r\n    secondaryUVMarginMethod: 1\r\n    secondaryUVMinLightmapResolution: 40\r\n    secondaryUVMinObjectScale: 1\r\n    secondaryUVPackMargin: 4\r\n    useFileScale: 1\r\n    strictVertexDataChecks: 0\r\n  tangentSpace:\r\n    normalSmoothAngle: 60\r\n    normalImportMode: 0\r\n    tangentImportMode: 3\r\n    normalCalculationMode: 4\r\n    legacyComputeAllNormalsFromSmoothingGroupsWhenMeshHasBlendShapes: 0\r\n    blendShapeNormalImportMode: 1\r\n    normalSmoothingSource: 0\r\n  referencedClips: []\r\n  importAnimation: 1\r\n  humanDescription:\r\n    serializedVersion: 3\r\n    human: []\r\n    skeleton: []\r\n    armTwist: 0.5\r\n    foreArmTwist: 0.5\r\n    upperLegTwist: 0.5\r\n    legTwist: 0.5\r\n    armStretch: 0.05\r\n    legStretch: 0.05\r\n    feetSpacing: 0\r\n    globalScale: 1\r\n    rootMotionBoneName: \r\n    hasTranslationDoF: 0\r\n    hasExtraRoot: 0\r\n    skeletonHasParents: 1\r\n  lastHumanDescriptionAvatarSource: {{instanceID: 0}}\r\n  autoGenerateAvatarMappingIfUnspecified: 1\r\n  animationType: 2\r\n  humanoidOversampling: 1\r\n  avatarSetup: 0\r\n  addHumanoidExtraRootOnlyWhenUsingAvatar: 1\r\n  importBlendShapeDeformPercent: 1\r\n  remapMaterialsIfMaterialImportModeIsNone: 0\r\n  additionalBone: 0\r\n  userData: \r\n  assetBundleName: \r\n  assetBundleVariant: \r\n";

        meshExporter.Export(exportSettings, exportPath);
        File.WriteAllText(Path.Combine(exportPath.directoryPath, name + ".prefab"), prefabFile.ToString());
        File.WriteAllText(Path.Combine(exportPath.directoryPath, name + ".obj.meta"), meshMetaFileTemplate);
    }


    private string GenerateFileId()
    {
        return random.NextLong().ToString();
    }

    private class PrefabInstance
    {
        public string prefabName;
        public string prefabGuid;
        public string prefabTransformFileId;
        public string prefabInstanceFileId;
        public string transformFileId;
        public string parentTranformComponentFileId;
        public Vector3 position;

        public PrefabInstance(string prefabName, string prefabGuid, string prefabTransformFileId, string prefabInstanceFileId, string transformFileId, string parentTranformComponentFileId, Vector3 position)
        {
            this.prefabName = prefabName;
            this.prefabGuid = prefabGuid;
            this.prefabTransformFileId = prefabTransformFileId;
            this.prefabInstanceFileId = prefabInstanceFileId;
            this.transformFileId = transformFileId;
            this.parentTranformComponentFileId = parentTranformComponentFileId;
            this.position = position;
        }

        public string GetParentTransformChildString()
        {
            return $"  - {{fileID: {transformFileId}}}\r\n";
        }

        public string GetPrefabInstanceString()
        {
            return $"--- !u!1001 &{prefabInstanceFileId}\r\nPrefabInstance:\r\n  m_ObjectHideFlags: 0\r\n  serializedVersion: 2\r\n  m_Modification:\r\n    serializedVersion: 3\r\n    m_TransformParent: {{fileID: {parentTranformComponentFileId}}}\r\n    m_Modifications:\r\n    - target: {{fileID: {prefabTransformFileId}, guid: {prefabGuid},\r\n        type: 3}}\r\n      propertyPath: m_LocalPosition.x\r\n      value: {-position.X - 0.5f}\r\n      objectReference: {{fileID: 0}}\r\n    - target: {{fileID: {prefabTransformFileId}, guid: {prefabGuid},\r\n        type: 3}}\r\n      propertyPath: m_LocalPosition.y\r\n      value: {position.Y + 0.5f}\r\n      objectReference: {{fileID: 0}}\r\n    - target: {{fileID: {prefabTransformFileId}, guid: {prefabGuid},\r\n        type: 3}}\r\n      propertyPath: m_LocalPosition.z\r\n      value: {position.Z + 0.5f}\r\n      objectReference: {{fileID: 0}}\r\n      m_RemovedComponents: []\r\n      m_RemovedGameObjects: []\r\n      m_AddedGameObjects: []\r\n      m_AddedComponents: []\r\n  m_SourcePrefab: {{fileID: 100100000, guid: {prefabGuid}, type: 3}}\r\n--- !u!4 &{transformFileId} stripped\r\nTransform:\r\n m_CorrespondingSourceObject: {{fileID: {prefabTransformFileId}, guid: {prefabGuid},\r\n  type: 3}}\r\n m_PrefabInstance: {{fileID: {prefabInstanceFileId}}}\r\n m_PrefabAsset: {{fileID: 0}}\r\n";
        }
    }
}