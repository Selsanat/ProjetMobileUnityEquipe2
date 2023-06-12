using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

namespace Map
{
    public class MapPlayerTracker : MonoBehaviour
    {
        public champSelector champSelection = null;
        public GameManager gameManager = null;
        public bool lockAfterSelecting = false;
        public float enterNodeDelay = 1f;
        public MapManager mapManager;
        public MapView view;
        public Campsite Camp;
        public Tower Tower;
        

        public static MapPlayerTracker Instance;

        public bool Locked { get; set; }

        private void Awake()
        {
            Instance = this;
            if (Camp == null)
                Camp = GameObject.Find("Campsite").GetComponent<Campsite>();
            Camp.gameObject.SetActive(false);
            if (Tower == null)
                Tower = GameObject.Find("Tour").GetComponent<Tower>();
            Tower.gameObject.SetActive(false);
            if (champSelection == null)
                champSelection = GameObject.Find("buttonManagerHero").GetComponent<champSelector>();
        }


        private void Start()
        {
            /*if(GameManager.Instance._currentNode != null && GameManager.Instance.waveCounter != mapManager.CurrentMap.path.Count)
                setPlayerToNode(GameManager.Instance._currentNode);*/
            
        }
        public void SelectNode(MapNode mapNode)
        {
            if (Locked) return;

            
            // Debug.Log("Selected node: " + mapNode.Node.point);

            if (mapManager.CurrentMap.path.Count == 0)
            {
                // player has not selected the node yet, he can select any of the nodes with y = 0
                if (mapNode.Node.point.y == 0)
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
            else
            {
                var currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
                var currentNode = mapManager.CurrentMap.GetNode(currentPoint);

                if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.Node.point)))
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
        }

        private void SendPlayerToNode(MapNode mapNode)
        {
            //_currentNode = mapNode;
            Locked = lockAfterSelecting;
            mapManager.CurrentMap.path.Add(mapNode.Node.point);
            mapManager.SaveMap();
            view.SetAttainableNodes();
            view.SetLineColors();
            mapNode.ShowSwirlAnimation();
            
            DOTween.Sequence().AppendInterval(enterNodeDelay).OnComplete(() => EnterNode(mapNode));
        }

        private void setPlayerToNode(MapNode mapNode)
        {
            Locked = lockAfterSelecting;
            mapManager.CurrentMap.path.Add(mapNode.Node.point);
            mapManager.SaveMap();
            view.SetAttainableNodes();
            view.SetLineColors();
        }

        private static void EnterNode(MapNode mapNode)
        {
            // we have access to blueprint name here as well
            Debug.Log("Entering node: " + mapNode.Node.blueprintName + " of type: " + mapNode.Node.nodeType);
            // load appropriate scene with context based on nodeType:
            // or show appropriate GUI over the map: 
            // if you choose to show GUI in some of these cases, do not forget to set "Locked" in MapPlayerTracker back to false
            switch (mapNode.Node.nodeType)
            {
                case NodeType.MinorEnemy:
                    Instance.transichamp();
                    break;
                case NodeType.EliteEnemy:
                    break;
                case NodeType.RestSite:
                    Instance.transicampfire();
                    break;
                case NodeType.Treasure:
                    break;
                case NodeType.Store:
                    break;
                case NodeType.Boss:
                    break;
                case NodeType.Mystery:
                    Instance.transitower();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IEnumerator  TransiChoixChamp()
        {
            GameManager gm = GameObject.FindObjectOfType<GameManager>();
            gm.transi.Play("Transi");
            yield return new WaitForSeconds(1.5f);
            gm.transi.Play("Detransi");
            GameObject.Find("OuterMapParent").SetActive(false);
            champSelection.setctive();
        }
        public IEnumerator TransiTower()
        {
            GameManager gm = GameObject.FindObjectOfType<GameManager>();
            gm.transi.Play("Transi");
            yield return new WaitForSeconds(1.5f);
            gm.transi.Play("Detransi");
            GameObject.Find("OuterMapParent").SetActive(false);
            Tower.gameObject.SetActive(true);
        }
        public IEnumerator TransiCampfire()
        {
            GameManager gm = GameObject.FindObjectOfType<GameManager>();
            gm.transi.Play("Transi");
            yield return new WaitForSeconds(1.5f);
            gm.transi.Play("Detransi");
            GameObject.Find("OuterMapParent").SetActive(false);
            Camp.gameObject.SetActive(true);
        }
        public void transicampfire()
        {
            StartCoroutine(TransiCampfire());
        }
        public void transichamp()
        {
            StartCoroutine(TransiChoixChamp());
        }
        public void transitower()
        {
            StartCoroutine(TransiTower());
        }

        private void PlayWarningThatNodeCannotBeAccessed()
        {
            Debug.Log("Selected node cannot be accessed");
        }
    }
}