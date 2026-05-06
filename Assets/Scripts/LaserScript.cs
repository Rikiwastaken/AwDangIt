using UnityEngine;

public class LaserScript : MonoBehaviour
{

    public float timebeforefadeout;
    private float opacitycounter;

    public float baseopacity;

    public void ResetMat()
    {
        gameObject.SetActive(true);
        GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, baseopacity);
        opacitycounter = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (opacitycounter < timebeforefadeout)
        {
            opacitycounter += Time.deltaTime;
            float alpha = baseopacity - baseopacity * (opacitycounter / timebeforefadeout);
            Color newcolor = GetComponent<Renderer>().material.color;

            newcolor.a = alpha;
            GetComponent<Renderer>().material.color = newcolor;
        }
        else
        {
            gameObject.SetActive(false);
        }

    }
}
