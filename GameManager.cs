using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nn;
    public List<GameObject> AIs = new List<GameObject>();
    public float stopwatch = 0f;

    [NonSerialized] public List<int> oInn = new List<int>();
    [NonSerialized] public List<int> iInn = new List<int>();
    [NonSerialized] public List<bool> RNN = new List<bool>();

    public int population = 100;
    void Start()
    {
        //creating population
        for(int i = 0; i < population; i++){
            float b = transform.position.x * Random.Range(0.9f, 1.1f);
            GameObject a = Instantiate(nn, new Vector3(b, transform.position.y, transform.position.z), Quaternion.identity);
            a.GetComponent<AI>().gm = gameObject;
            a.GetComponent<AI>().addition = 1;
            //a.GetComponent<AI>().Conn = 10;
            AIs.Add(a);
        }
        AIs[0].GetComponent<AI>().Conn = 1;
        //Time.timeScale = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if(stopwatch < 1){
            AIs.Clear();
            AIs.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        }
        stopwatch += Time.deltaTime;
        if(stopwatch >= 8){
            stopwatch = 0.1f;
            List<GameObject> SortedList = AIs.OrderByDescending(o=>o.GetComponent<AI>().layer + (o.transform.position.z * 0.0001)).ToList();
            Debug.Log(AIs.Count);
            List<GameObject> copyAI = new List<GameObject>();
            foreach(GameObject a in AIs){
                copyAI.Add(a);
            }
            Debug.Log(copyAI.Count);
            AIs.Clear();
            Debug.Log(copyAI.Count);
            List<GameObject> NewAIs = SortedList.GetRange(0, 6);
            foreach(var a in NewAIs){
                float c = transform.position.x * Random.Range(0.9f, 1.1f);
                var b = Instantiate(a, new Vector3(c, transform.position.y, transform.position.z), Quaternion.identity);
                b.name = "nn";
            }
            //for(int i = 0; i < population; i++)
            {
                for(int ii = 0; ii < population - 6; ii++){
                    float b = transform.position.x * Random.Range(0.9f, 1.1f);
                    int FirstInd = Random.Range(0, 5);
                    GameObject offspring = SortedList[FirstInd];
                    GameObject c = SortedList[Random.Range(FirstInd + 1, 6)];
                    offspring.GetComponent<AI>().Conn = SortedList[FirstInd].GetComponent<AI>().Conn;
                    //b.GetComponent<AI>().Conn.Add(6);

                    //Random inheritance of weights
                    if(offspring.GetComponent<AI>().innovations.Any()){
                        if(c.GetComponent<AI>().innovations.Any()){
                            for(int iii = 0; iii < offspring.GetComponent<AI>().innovations.Count; iii++){
                                if(Random.Range(0, 2) == 0){
                                        int tryFind = c.GetComponent<AI>().innovations.IndexOf(offspring.GetComponent<AI>().innovations[iii]);
                                        if(tryFind != -1){
                                            try{
                                                offspring.GetComponent<AI>().weights[iii] = c.GetComponent<AI>().weights[tryFind];
                                            }
                                            catch{}
                                        }
                                }
                            }
                        }
                    }
                    if(Random.Range(0, 5) == 0){
                        offspring.GetComponent<AI>().addition = 1;
                    }
                    else if(Random.Range(0, 5) == 0){
                        offspring.GetComponent<AI>().addition = 2;
                    }
                    else{
                        offspring.GetComponent<AI>().addition = 0;
                    }
                    //Reactivate and deactivate
                    if(offspring.GetComponent<AI>().actConnect.Any()){
                        if(Random.Range(0, 5) < 1){
                            //offspring.GetComponent<AI>().actConnect[Random.Range(0, offspring.GetComponent<AI>().actConnect.Count - 1)] = false;
                            //offspring.GetComponent<AI>().actConnect.RemoveAt(Random.Range(0, offspring.GetComponent<AI>().actConnect.Count - 1));
                        }
                        else if(Random.Range(0, 5) < 1){
                            List<int> check = Enumerable.Range(0, offspring.GetComponent<AI>().actConnect.Count)
                                .Where(i => offspring.GetComponent<AI>().actConnect[i] == offspring.GetComponent<AI>().actConnect.Last())
                                .ToList();
                            offspring.GetComponent<AI>().actConnect[check[Random.Range(0, check.Count)]] = true;
                        }
                    }
                    for(int ie = 0; ie < offspring.GetComponent<AI>().weights.Count; ie++){
                        if(Random.Range(0, 5) < 4){
                            offspring.GetComponent<AI>().weights[ie] = offspring.GetComponent<AI>().weights[ie] * Random.Range(-1.5f, 1.5f);
                        }
                    }
                    NewAIs.Add(offspring);
                    /*GameObject a = (GameObject) Instantiate(NewAIs[ii], new Vector3(b, transform.position.y, transform.position.z), Quaternion.identity);
                    a.GetComponent<AI>().Adder();
                    //a.name = "nn";*/
                    var a = Instantiate(NewAIs[ii], new Vector3(b, transform.position.y, transform.position.z), Quaternion.identity);
                    a.name = "nn";
                    //Debug.Log("here");
                }
            }
            Debug.Log(copyAI.Count);
            foreach(var a in copyAI){
                Destroy(a);
            }
            /*foreach(var f in AIs){
                f.GetComponent<AI>().enabled = true;
            }*/
        }
    }
    public int DealInnovations(int inoV, int outV, bool rnnV){
        {
            for(int i = 0; i < iInn.Count; i++){
                if(iInn[i] == inoV){
                    if(oInn[i] == outV){
                        if(RNN[i] == rnnV){
                            return i;
                        }
                    } 
                }
            }
        }
        iInn.Add(inoV);
        oInn.Add(outV);
        RNN.Add(rnnV);
        return iInn.Count - 1;
    }
}

