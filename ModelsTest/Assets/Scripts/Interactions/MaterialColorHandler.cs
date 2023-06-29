namespace Interactions
{
    using UnityEngine;

    public class MaterialColorHandler : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private Color[] colors;

        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        
        private MaterialPropertyBlock[] _collectedMaterialPropertyBlocks;

        private SkinnedMeshRenderer[] _meshRenderers;

        private void Start() => CollectMaterials();

        public void Interact() => ChangeColor();
        
        public void ChangeColor()
        {
            for (var i = 0; i < _collectedMaterialPropertyBlocks.Length; i++)
            {
                var block = _collectedMaterialPropertyBlocks[i];
                var randomColorIndex = Random.Range(0, colors.Length);

                var randomColor = colors[randomColorIndex];

                block.SetColor(BaseColor, randomColor);
                _meshRenderers[i].SetPropertyBlock(block);
            }
        }

        private void CollectMaterials()
        {
            _meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            _collectedMaterialPropertyBlocks = new MaterialPropertyBlock[_meshRenderers.Length];

            for (var i = 0; i < _meshRenderers.Length; i++)
            {
                var block = new MaterialPropertyBlock();
                _meshRenderers[i].GetPropertyBlock(block);
                
                _collectedMaterialPropertyBlocks[i] = block;
            }
        }
    }
}
