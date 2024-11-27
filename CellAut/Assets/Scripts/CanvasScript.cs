using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] private GenerateSpriteGrid genSpriteGrid;

    [SerializeField] private Slider genTimeSlider;
    [SerializeField] private TextMeshProUGUI sliderValText;


    private void Start()
    {
        genTimeSlider.value = genSpriteGrid.GetAutoGenTime();
    }

    private void Update()
    {
        genTimeSlider.gameObject.SetActive(genSpriteGrid.GetIsAuto());
    }

    public void Slider()
    {
        sliderValText.text = System.Math.Round(genTimeSlider.value, 2).ToString();

        genSpriteGrid.SetAutoGenTime(genTimeSlider.value);
    }
}
