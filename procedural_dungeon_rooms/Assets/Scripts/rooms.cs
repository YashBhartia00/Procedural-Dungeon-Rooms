using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rooms : MonoBehaviour
{
        GameObject[] Rooms;
        List<GameObject> redRooms = new List<GameObject>();
        public GameObject RoomPrefab, roomParent, pointPrefab, boxPrefab;
        public bool checkmoving,  tempBool= true, moving = false, finishMoving = false, triangulated = false;
        public static float maxX, MaxY, unit;
        int numRooms;
        int [,] connections;
        int [][] paths;
        
        private void Start()
        {
        unit = 0.5f;
        numRooms = 200;
        maxX = numRooms * 3;
        MaxY = numRooms ;
        Rooms = new GameObject[numRooms];
        StartCoroutine(spawnRooms(numRooms));
        // fillWithBoxes(new Vector3(0,0,0), new Vector3(3.75f,-2.5f));
        }
        private void Update()
        {
          if (checkmoving)
            {
                int brool = 0;
                for (int i = 0; i < numRooms; i++)
                {

                    if (!Rooms[i].GetComponent<Rigidbody2D>().IsAwake()) {brool += 1; }
                }
                if (brool >= numRooms) { 
                    checkmoving = false; moving = false; 
                    StartCoroutine(triangulate());
                    // maxX = Mathf.Abs(Physics2D.BoxCast(new Vector3(-maxX,0,0),new Vector2(1,MaxY*2),0,new Vector2(1,0)).collider.gameObject.transform.position.x) +7;
                    // MaxY = Mathf.Abs(Physics2D.BoxCast(new Vector3(0,-MaxY,0),new Vector2(maxX*4,1),0,new Vector2(0,1)).collider.gameObject.transform.position.y) +7;
                    //for the maxes maybe compare from left and right but since our maintain grid shifts it all downwards its okay
                    for(int i = 0 ; i<numRooms;i++)
                    {
                        Rooms[i].GetComponent<Rigidbody2D>().isKinematic = true;
                    }
                    // fillWithBoxes(new Vector3(maxX,MaxY,0),new Vector3(-maxX,-MaxY,0));

                 }
            }
        }
        IEnumerator triangulate()
        {
            DelaunayVoronoi.DelaunayTriangulator umok = new DelaunayVoronoi.DelaunayTriangulator();
            var ieTriangles = umok.BowyerWatson(umok.GeneratePoints(maxX,MaxY,redRooms));
            foreach (var triangle in ieTriangles)
            {
                //Instantiate(pointPrefab, new Vector3(float.Parse(triangle.Vertices[ij].X.ToString("0.0000"))-200, float.Parse(triangle.Vertices[ij].Y.ToString("0.0000"))-40, 0), Quaternion.identity);
                var x1 = float.Parse(triangle.Vertices[0].X.ToString("0.0000")) - maxX/2;
                var y1 = float.Parse(triangle.Vertices[0].Y.ToString("0.0000")) - MaxY / 2;

                var x2 = float.Parse(triangle.Vertices[1].X.ToString("0.0000")) - maxX / 2;
                var y2 = float.Parse(triangle.Vertices[1].Y.ToString("0.0000")) - MaxY/2;

                var x3 = float.Parse(triangle.Vertices[2].X.ToString("0.0000")) - maxX / 2;
                var y3 = float.Parse(triangle.Vertices[2].Y.ToString("0.0000")) - MaxY/2;

                Debug.DrawLine(new Vector3(x1,y1,0),new Vector3(x2,y2,0),Color.red,5,false);
                Debug.DrawLine(new Vector3(x1, y1, 0), new Vector3(x3, y3, 0), Color.red,5, false);
                Debug.DrawLine(new Vector3(x3, y3, 0), new Vector3(x2, y2, 0), Color.red, 5, false);

                for(int i =0;i<redRooms.Count; i++)
                {
                  if(redRooms[i].gameObject.transform.position == new Vector3(x1,y1,0)) 
                   {
                      for (int j = 0; j<redRooms.Count;j++)
                       {
                            if((redRooms[j].gameObject.transform.position == new Vector3(x2,y2,0))||
                                (redRooms[j].gameObject.transform.position == new Vector3(x3,y3,0))) 
                            {
                                connections[i,j] = 1;
                                // print(i+","+j+ ": " + connections[i,j]);
                            }
                       }
                   }
                  if(redRooms[i].gameObject.transform.position == new Vector3(x2,y2,0)) 
                   {
                       for (int j = 0; j<redRooms.Count;j++)
                       {
                            if((redRooms[j].gameObject.transform.position == new Vector3(x1,y1,0))||
                                (redRooms[j].gameObject.transform.position == new Vector3(x3,y3,0))) 
                            {
                                connections[i,j] = 1;
                                // print(i+","+j+ ": " + connections[i,j]);
                            }
                       }
                   }
                  if(redRooms[i].gameObject.transform.position == new Vector3(x3,y3,0)) 
                   {
                       for (int j = 0; j<redRooms.Count;j++)
                       {
                            if((redRooms[j].gameObject.transform.position == new Vector3(x1,y1,0))||
                                (redRooms[j].gameObject.transform.position == new Vector3(x2,y2,0))) 
                            {
                                connections[i,j] = 1;
                                // print(i+","+j+ ": " + connections[i,j]);
                            }
                       }
                   }
                }

        }
        StartCoroutine(pathFinder());
        yield return null;
        }
        IEnumerator spawnRooms(int numRooms)

    {
        for ( int i =0 ;i <numRooms; i++)
           {
            Rooms[i] = Instantiate(RoomPrefab, getRandomPointsInEllipse(numRooms/20, 4f), Quaternion.identity, roomParent.transform);
            if(Rooms[i].transform.position == new Vector3(0,0,0))
            { Rooms[i].transform.position += new Vector3(1,1,0)*unit;}
            Rooms[i].transform.localScale = new Vector3(Mathf.Floor(normalRandom(6f, 4f)), Mathf.Floor(normalRandom(6f, 4f)), 0)/2;
            while(Rooms[i].transform.localScale.x<= 0.7f ||Rooms[i].transform.localScale.y<= 0.7f)
            {
            Rooms[i].transform.localScale = new Vector3(Mathf.Floor(normalRandom(6f, 4f)), Mathf.Floor(normalRandom(6f, 4f)), 0)/2;
            }
           }
        for (int i = 0; i < numRooms; i++)
        {
            var rb = Rooms[i].GetComponent<BoxCollider2D>();
            Rooms[i].GetComponent<maintainGrid>().enabled = true;
            rb.enabled = true;
        }
        yield return new WaitForSeconds(3);
        for (int i = 0; i < numRooms; i++)
        {
            if (Rooms[i].transform.localScale.x > 4f && Rooms[i].transform.localScale.y > 4f)
            {
                Rooms[i].GetComponent<SpriteRenderer>().color = Color.red;
                print(redRooms.Count);
                redRooms.Add(Rooms[i]);
                Rooms[i].tag = "MainRoom";
                yield return new WaitForSeconds(0.05f);
            }

        }
        checkmoving = true;
        connections= new int[redRooms.Count,redRooms.Count];
        //yield return null;
    }//max x = 70*2 , y = 3*2

    // math functions
        float roundm(float n, float m)
        {
            return Mathf.Floor(((n + m - 1) / m)) / m;
        }
        float normalRandom(float mean, float stdDev)
        {
            System.Random rand = new System.Random(); //reuse this if you are generating many
            float u1 = 1.0f - Random.Range(0f, 1f); //uniform(0,1] random floats
            float u2 = 1.0f - Random.Range(0f, 1f);
            float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                            Mathf.Sin(2.0f * Mathf.PI * u2); //random normal(0,1)
            float randNormal =
                            mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
            return randNormal;
        }
        Vector2 getRandomPointsInEllipse(float ellipse_width, float ellipse_height)
        {
            float t = 2 * Mathf.PI * Random.Range(0.1f, 1.1f);
            float u = Random.Range(0.1f, 1.1f) + Random.Range(0.1f, 1.1f);
            float r;
            if (u > 1) { r = 2 - u; } else { r = u; }
            return new Vector2(roundm(ellipse_width * r * Mathf.Cos(t) / 2, 0.5f),
                roundm(ellipse_height * r * Mathf.Sin(t) / 2, 0.5f));

        }


        IEnumerator plotL(int A, int B,int iter) //greenlines,ymlines,boxgen,final 
        {
            if(iter ==1)
            {
             Debug.DrawLine(redRooms[A].transform.position,redRooms[B].transform.position,Color.green,0.5f);
            }

            Vector3 a = redRooms[A].transform.position;
            Vector3 b = redRooms[B].transform.position;
            Vector3 y = new Vector3(b.x,a.y,0);
            Vector3 m = new Vector3(a.x,b.y,0);
            bool ym /*01*/ = false;
            int coun=0;
            RaycastHit2D []hitsn;
            RaycastHit2D []hitsBn;


            RaycastHit2D []hits =  Physics2D.RaycastAll(new Vector3(b.x,a.y,0),new Vector2(0,Mathf.Sign(b.y-a.y)),Vector3.Distance(new Vector3(b.x,a.y,0),b));
            RaycastHit2D []hitsB =  Physics2D.RaycastAll(a,new Vector2(Mathf.Sign(b.x-a.x),0), Vector3.Distance(new Vector3(b.x,a.y,0),a));
            
            for(int i =0 ; i < hits.Length;i++)
            {
                if(hits[i].collider.gameObject.tag == "MainRoom")
                {
                    coun ++;
                }
            }
            for(int i =0 ; i < hitsB.Length;i++)
            {
                if(hitsB[i].collider.gameObject.tag == "MainRoom")
                {
                    coun ++;
                }
            }
            if(coun>2)
            {
                ym=true;
            }

            if(!ym)
            {
                if(iter ==2)
                {
                    Debug.DrawLine(a,new Vector3(b.x,a.y,0),Color.yellow,0.5f);
                    Debug.DrawLine(new Vector3(b.x,a.y,0),b,Color.yellow,0.5f);
                }

                if(iter ==3)
                {
                fillWithBoxes(a-new Vector3(0,0.5f,0),y+new Vector3(0,0.5f,0));
                fillWithBoxes(y- new Vector3(0.5f,0,0),b+new Vector3(0.5f,0,0));
                }

                hitsn =  Physics2D.RaycastAll(new Vector3(b.x,a.y,0),new Vector2(0,Mathf.Sign(b.y-a.y)),Vector3.Distance(new Vector3(b.x,a.y,0),b));
                hitsBn =  Physics2D.RaycastAll(a,new Vector2(Mathf.Sign(b.x-a.x),0), Vector3.Distance(new Vector3(b.x,a.y,0),a));
            }else
            {
                if(iter ==2)
                {
                    Debug.DrawLine(a,new Vector3(a.x,b.y,0),Color.magenta,0.5f);
                    Debug.DrawLine(new Vector3(a.x,b.y,0),b,Color.magenta,0.5f);
                }

                if(iter ==3)
                {
                fillWithBoxes(a-new Vector3(0.5f,0,0),m+new Vector3(0.5f,0,0));
                fillWithBoxes(m- new Vector3(0,0.5f,0),b+new Vector3(0,0.5f,0));
                }

                hitsn =  Physics2D.RaycastAll(a,new Vector2(0,Mathf.Sign(m.y-a.y)),Vector3.Distance(a,m));
                hitsBn =  Physics2D.RaycastAll(m,new Vector2(Mathf.Sign(b.x-m.x),0),Vector3.Distance(b,m));
            }

            if(iter ==4)
            {
                for(int i =0 ; i < hitsn.Length;i++)
                {
                    hitsn[i].collider.gameObject.GetComponent<SpriteRenderer>().color =Color.white;
                    if(hitsn[i].collider.gameObject.tag == "Box")
                    {
                        hitsn[i].collider.gameObject.GetComponent<SpriteRenderer>().color = ym ? Color.magenta : Color.yellow;
                    }

                    if(hitsn[i].collider.gameObject.tag == "MainRoom")
                    {
                    hitsn[i].collider.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                    }
                    StartCoroutine(blackify(hitsn[i].collider.gameObject));
                }
                for(int i =0 ; i < hitsBn.Length;i++)
                {
                    hitsBn[i].collider.gameObject.GetComponent<SpriteRenderer>().color =Color.white;
                    if(hitsBn[i].collider.gameObject.tag == "Box")
                    {
                        hitsBn[i].collider.gameObject.GetComponent<SpriteRenderer>().color = ym ? Color.magenta : Color.yellow;
                    }
                    if(hitsBn[i].collider.gameObject.tag == "MainRoom")
                    {
                    hitsBn[i].collider.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                    }
                    StartCoroutine(blackify(hitsBn[i].collider.gameObject));
                }
            }
            yield return null;
        }
        IEnumerator blackify(GameObject obj)
        {
            yield return new WaitForSeconds(2f);
            obj.GetComponent<SpriteRenderer>().color= Color.black;
            if(obj.tag == "MainRoom")
            {
            obj.GetComponent<SpriteRenderer>().color= Color.red;
            }
        }
        IEnumerator pathFinder()
        {
            // pathFinding.DijkstrasAlgorithm dj = new pathFinding.DijkstrasAlgorithm();
            // dj.dijkstra(connections,0);             
            pathFinding.Graph g = new pathFinding.Graph(redRooms.Count);
            
            print("Adding edges");
            for(int i = 0;i<redRooms.Count;i++)
            {
                for(int j=0;j<redRooms.Count;j++)
                {
                    if(connections[i,j]!=0 && i!=j)
                    {
                        g.addEdge(i,j);
                        // print(i+"E"+j);
                        // g.addEdge(j,i);
                    }
                }
            }
        
            int s = 0;
            int d = redRooms.Count-1;
            Debug.Log("Following are all different" +  
                                " paths from " + s + " to " + d);  
            g.printAllPaths(s, d);  
            print("done");
            yield return new WaitForSeconds(10);
            paths = new int[g.numbersOnly.Count][];

            for(int i =0; i <g.numbersOnly.Count;i++)
            {
                string[] temp = g.numbersOnly[i].Split(' ');
                paths[i] = new int [temp.Length];
                for(int j = 0; j<temp.Length;j++)
                {
                    for(int k = 0 ; k<temp.Length;k++)
                    {
                        if(temp[j]!=  temp[k])
                       {paths[i][j] = int.Parse(temp[j]);} else if(j==k)
                       {continue;}else {
                           for(int h =0;h<temp.Length;h++)
                           {
                               paths[i][h]= 0;
                           }
                       }
                    }
                }
            }

            for(int j = 0; j<paths.Length;j++ )
            {
                bool badPath = false;
                Debug.Log(System.String.Join(", ", new List<int>(paths[j]) .ConvertAll(i => i.ToString()) .ToArray()));
                for(int i = 0 ; i<paths[j].Length -1;i++)
                {
                    if(paths[j][1]==0){badPath = true;continue;}
                   StartCoroutine( plotL(paths[j][i],paths[j][i+1],1));
                }
                yield return new WaitForSeconds(0.5f);
                for(int i = 0 ; i<paths[j].Length -1;i++)
                {
                    if(paths[j][1]==0){badPath = true;continue;}
                   StartCoroutine( plotL(paths[j][i],paths[j][i+1],2));
                }
                yield return new WaitForSeconds(0.5f);
                for(int i = 0 ; i<paths[j].Length -1;i++)
                {
                    if(paths[j][1]==0){badPath = true;continue;}
                   StartCoroutine( plotL(paths[j][i],paths[j][i+1],3));
                }
                // yield return new WaitForSeconds(0.5f);
                for(int i = 0 ; i<paths[j].Length -1;i++)
                {
                    if(paths[j][1]==0){badPath = true;continue;}
                   StartCoroutine( plotL(paths[j][i],paths[j][i+1],4));
                }
                if(badPath){}else{
                // halls(paths[j]);
                yield return new WaitForSeconds(2f);
                }
            }
        } 
         
        void fillWithBoxes(Vector3 boxD1, Vector3 boxD2)
        {
            // print("something");
            float size = unit/2;
            float dirX = Mathf.Sign(boxD2.x- boxD1.x);
            float dirY = Mathf.Sign(boxD2.y- boxD1.y);
            for(int i =0; i<Mathf.Abs(boxD1.x-boxD2.x)/size;i++)
            {
                for(int j =0 ; j<Mathf.Abs(boxD1.y - boxD2.y)/size;j++)
                {
                    if(Physics2D.RaycastAll(boxD1 + new Vector3(dirX*size/2,dirY*size/2,0) + size*(new Vector3(dirX*i,dirY*j,0)),new Vector2(1,0),size/8).Length<1)
                    {
                        var a = Instantiate(boxPrefab,boxD1 + new Vector3(dirX*size/2,dirY*size/2,0) + size*(new Vector3(dirX*i,dirY*j,0)) ,Quaternion.identity,pointPrefab.transform) ;
                        a.transform.localScale = new Vector3(0.25f,0.25f,1);
                        // a.GetComponent<Rigidbody2D>().
                        a.GetComponent<BoxCollider2D>().enabled = true;
                        a.GetComponent<Rigidbody2D>().isKinematic= true;
                    }
                }
            }
       }
}