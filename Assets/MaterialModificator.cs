using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialModificator : MonoBehaviour
{

    private List<Material> materialsList;

    public float maxveg = 0.4f;
    public float maxCorruption = 0.4f;
    public float timetosetVeg;
    public float timetosetCorruption;

    // Start is called before the first frame update
    void Start()
    {
        materialsList = new List<Material>();
        FillMaterialList(gameObject, materialsList);
        SetCorruption(0f);
        SetVegetation(0f);
        StartCoroutine(SetVegetationToValue(1.0f));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator SetVegetationToValue(float targetpercent)
    {
        float targetTime = Time.time + timetosetVeg;
        float clampedpercent = targetpercent * maxveg;
        if (materialsList == null)
        {
            FillMaterialList(gameObject, materialsList);
        }
        float vegetationtoapply = GetVegetation();
        while (Time.time < targetTime)
        {
            vegetationtoapply = Mathf.Lerp(vegetationtoapply, clampedpercent, 1f - (targetTime - Time.time) / timetosetVeg);

            SetVegetation(vegetationtoapply);
            yield return true;
        }
    }

    private void SetVegetation(float value)
    {
        foreach (Material mat in materialsList)
        {
            mat.SetFloat("_StepIntensity", value);
        }
    }

    private IEnumerator SetCorruptionToValue(float targetpercent)
    {
        float clampedpercent = targetpercent * maxCorruption;

        if (materialsList == null)
        {
            FillMaterialList(gameObject, materialsList);
        }
        float basecorruption = GetCorruption();
        Debug.Log(basecorruption);
        float timeelapsed = 0f;
        while (timeelapsed < timetosetCorruption)
        {
            float ratio = timeelapsed / timetosetCorruption;
            float corruptiontoapply = basecorruption + (clampedpercent - basecorruption) * ratio;

            SetCorruption(corruptiontoapply);
            timeelapsed += Time.deltaTime;
            yield return true;
        }
    }

    private void SetCorruption(float value)
    {
        foreach (Material mat in materialsList)
        {
            mat.SetFloat("_StainStepIntensity", value);
        }
    }

    private float GetVegetation()
    {
        if (materialsList != null && materialsList.Count > 0)
        {
            return materialsList[0].GetFloat("_StepIntensity");
        }
        return 0f;
    }

    private float GetCorruption()
    {
        if (materialsList != null && materialsList.Count > 0)
        {
            return materialsList[0].GetFloat("_StainStepIntensity");
        }
        return 0f;
    }

    private void FillMaterialList(GameObject go, List<Material> matList)
    {
        if (go == null)
        {
            return;
        }
        if (matList == null)
        {
            matList = new List<Material>();
        }
        if (go.GetComponent<Renderer>())
        {
            matList.Add(go.GetComponent<Renderer>().material);
        }
        foreach (Transform child in go.transform)
        {
            FillMaterialList(child.gameObject, matList);
        }
    }
#if UNITY_EDITOR
    [ContextMenu("Set Corruption to max")]
    void MaxCorr()
    {
        StartCoroutine(SetCorruptionToValue(1.0f));
    }

    [ContextMenu("Set Corruption to 0")]
    void LowCorr()
    {
        StartCoroutine(SetCorruptionToValue(0.0f));
    }

    [ContextMenu("Set Veg to max")]
    void MaxVeg()
    {
        StartCoroutine(SetVegetationToValue(1.0f));
    }

    [ContextMenu("Set Veg to 0")]
    void LowVeg()
    {
        StartCoroutine(SetVegetationToValue(0.0f));
    }
#endif
}
