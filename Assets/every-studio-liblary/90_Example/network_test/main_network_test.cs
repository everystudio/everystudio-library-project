using UnityEngine;
using System.Collections;

public class main_network_test : MonoBehaviour {

	// Use this for initialization
	void Start () {

		CommonNetwork.Instance.Recieve ("http://192.168.33.10/CodeIgniter-3.0.1/index.php/sample");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
