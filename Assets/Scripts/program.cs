using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
public class program : MonoBehaviour {

    public GameObject cube_prefab;
    private bool instantiated = false;
    public Slider s;
    private GameObject[] cube_array;
    private float[] heights;
    public Dropdown algorithms;
    private bool clicked = false, sorted = false, merged = false, test = false, reverse = false;
    private int i_b, i_i, j_b, j_i, i_s, i_g, i_c, ms_count = 0 ,swaps = 0, middle = 0;
    public Material mat_blue, mat_white;
    public Text swap_count_text, slider_text;

    private float[,] states;
    private int state_index = 0;

    private List<int> dummy;

    private Thread t1;

    private void Start() {

        i_b = 0;
        j_b = 0;
        i_i = 1;
        j_i = i_i - 1;
        i_s = 0;
        i_g = 0;
        i_c = 0;

        RenderBars((int)s.value);
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        if (!sorted && clicked)
            OnClickSort();
    }

    public void OnClickSort() {        
        clicked = true;
        string algorithm;
        algorithm = algorithms.options[algorithms.value].text;
        switch (algorithm) {
            case "Bubble sort":
                BubbleSort();
                break;
            case "Insertion sort":
                InsertionSort();
                break;
            case "Selection sort":
                SelectionSort();
                break;
            case "Gnome sort":
                GnomeSort();
                break;
            case "Cocktail sort":
                CocktailSort();
                break;
            default:
                break;
        }
     }

    //***Sorting Algorithms***

    private void BubbleSort() {     //Compare adjacent elemtns and swap them
        float[] temp = new float[heights.Length];
        Array.Copy(heights, 0, temp, 0, heights.Length);
        Array.Sort(temp);

        if (i_b < heights.Length - 1) {
            if (j_b < heights.Length - i_b - 1) {
                if (heights[j_b] > heights[j_b + 1]) {
                    float t = heights[j_b];
                    heights[j_b] = heights[j_b + 1];
                    heights[j_b + 1] = t;
                    Swap(j_b, j_b + 1);
                }
                j_b += 1;
            }
            else {
                j_b = 0;
                i_b += 1;
            }
            ChangeColor(temp);
        }
    }


    private void InsertionSort() {  //Insertion sort is like sorting cards in hand
        float[] temp = new float[heights.Length];
        Array.Copy(heights, 0, temp, 0, heights.Length);
        Array.Sort(temp);

        if (i_i < heights.Length){
            if (j_i >= 0 && heights[j_i] > heights[j_i + 1]) {
                float t = heights[j_i];
                heights[j_i] = heights[j_i + 1];
                heights[j_i + 1] = t;
                Swap(j_i, j_i + 1);
                j_i -= 1;
            }
            else {
                i_i += 1;
                j_i = i_i - 1;
            }
            ChangeColor(temp);
        }
    }

    private void SelectionSort() {  // Starting from 0, it parses the array and swaps the smallest element with current element

        float[] temp = new float[heights.Length];
        Array.Copy(heights, 0, temp, 0, heights.Length);
        Array.Sort(temp);

        if (i_s < heights.Length) {
            float min = heights[i_s];
            int min_index = i_s;
            for (int i = i_s; i < heights.Length; i++) {
                if (heights[i] < min) {
                    min = heights[i];
                    min_index = i;
                }
            }
            if (i_s != min_index) {
                float t = heights[i_s];
                heights[i_s] = heights[min_index];
                heights[min_index] = t;
                Swap(i_s, min_index);
            }
            i_s += 1;
            ChangeColor(temp);
        }
    }

    private void GnomeSort() {  //Compare adjacent elemetns and swap and go back and check all elements before are sorted 
        if (i_g < heights.Length) {

            float[] temp = new float[heights.Length];
            Array.Copy(heights, 0, temp, 0, heights.Length);
            Array.Sort(temp);

            if (i_g == 0)
                i_g += 1;
            if (heights[i_g] >= heights[i_g - 1]) {
                i_g += 1;

            }
            else {
                Swap(i_g, i_g - 1);
                float t = heights[i_g];
                heights[i_g] = heights[i_g - 1];
                heights[i_g - 1] = t;
                i_g -= 1;
            }
            ChangeColor(temp);
        }
    }

    private void CocktailSort() {   //Similar to Bubble sort, travels in both direction

        float[] temp = new float[heights.Length];
        Array.Copy(heights, 0, temp, 0, heights.Length);
        Array.Sort(temp);

        if (i_c < heights.Length - 1 && !reverse) {
            if (heights[i_c] > heights[i_c + 1]) {
                float t = heights[i_c];
                heights[i_c] = heights[i_c + 1];
                heights[i_c + 1] = t;
                Swap(i_c, i_c + 1);
            }
            i_c += 1;
        }
        if (i_c == heights.Length - 1)
            reverse = true;

        if (i_c > 0 && reverse) {
            if (heights[i_c] < heights[i_c - 1]) {
                float t = heights[i_c];
                heights[i_c] = heights[i_c - 1];
                heights[i_c - 1] = t;
                Swap(i_c, i_c - 1);
            }
            i_c -= 1;
        }
        if (i_c == 0)
            reverse = false;

        ChangeColor(temp);

    }

    //*** Helper functions***
    private void Swap(int a, int b) {
        GameObject t = cube_array[a];
        cube_array[a] = cube_array[b];
        cube_array[b] = t;
        Vector3 v = cube_array[a].transform.position;
        cube_array[a].transform.position = new Vector3(cube_array[b].transform.position.x, v.y, 0f);
        cube_array[b].transform.position = new Vector3(v.x, cube_array[b].transform.position.y, 0f);
        swaps += 1;
        swap_count_text.text = "Swaps: " + swaps;
    }

    private void ChangeColor(float[] s) {
        for (int k = 0; k < heights.Length; k++) {
            Renderer rend = cube_array[k].GetComponent<Renderer>();
            if (heights[k] == s[k])
                rend.material = mat_blue;
            else
                rend.material = mat_white;
        }
    }
    public void OnSliderMoved() {
        Reset();
        RenderBars((int)s.value);
    }

    public void RenderBars(int n) {

        slider_text.text = "Number of items: " + n;
        if (instantiated) {            
            for (int i = 0; i < cube_array.Length; i++) {
                Destroy(cube_array[i]);
                heights[i] = -1;
            }
        }
        cube_array = new GameObject[n];
        heights = new float[n];
        float thickness = (130 - ((n - 1) * 0.5f)) / (n - 1);
        float start = -65f;
        float z = 0f;
        for (int i = 0; i < n; i++) {
            heights[i] = UnityEngine.Random.Range(5, 75);
            cube_array[i] = Instantiate(cube_prefab, new Vector3(start, (heights[i] / 2) - 35f, z), Quaternion.identity);
            cube_array[i].transform.localScale = new Vector3(thickness, heights[i], 1f);
            start += thickness + 0.5f;
        }
        instantiated = true;

        //r = heights.Length - 1;
    }

    public void Reset() {
        i_b = 0;
        j_b = 0;
        i_i = 1;
        j_i = i_i - 1;
        i_s = 0;
        i_g = 0;
        i_c = 0;
        swaps = 0;
        sorted = false;
        clicked = false;
        reverse = false;
        swap_count_text.text = "Swaps: ";
        for (int k = 0; k < heights.Length; k++) {
            Renderer rend = cube_array[k].GetComponent<Renderer>();
            rend.material = mat_white;
        }
    }
}
