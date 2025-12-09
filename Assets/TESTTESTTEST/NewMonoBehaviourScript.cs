using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Texture tex1,tex2;
    Material mat;
    float spriteCounter = 1;
    bool firstSprite = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        spriteCounter += Time.deltaTime;
        if(spriteCounter > 1)
        {
            spriteCounter = 0;
            firstSprite = !firstSprite;
            mat.mainTexture = firstSprite ? tex1 : tex2;
        }
    }
}
