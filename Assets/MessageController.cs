using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    private Image _image;
    public float ShowTime;
    public Text _text;

    // Use this for initialization
    void Start()
    {
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator ShowMessage(string msg)
    {
        _text.text = msg;
        
        // fade from transparent to opaque
        
        // loop over 1 second
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            _image.color = new Color(1, 1, 1, i);
            var c = _text.color;
            _text.color = new Color(c.r, c.g, c.b, i);
            yield return null;
        }
        
        yield return new WaitForSeconds(ShowTime);
        // fade from opaque to transparent

        // loop over 1 second backwards
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            _image.color = new Color(1, 1, 1, i);
            var c = _text.color;
            _text.color = new Color(c.r, c.g, c.b, i);
            yield return null;
        }
    }
}