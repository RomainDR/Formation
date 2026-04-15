using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace __GameAssets__.Assets._00_Prog.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text lifeCounter;
        [SerializeField] private Transform panel;
        [SerializeField] private Button reanimeButton;

        public static UIManager Instance { get; private set; }

        private void Awake()
        {
            // S'assure qu'il n'y a qu'une seule instance
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            reanimeButton.onClick.AddListener(Reanime);
        }

        private void Reanime()
        {
            GameManager.Instance.ResetPlayer();

            Player player = GameManager.Instance.GetPlayer();
            player.SetLife(player.GetStartLife());
        }

        public void SetLifeCounter(int _amount)
        {
            lifeCounter.text = "Life: " + _amount;
        }

        public void ShowDeathPanel()
        {
            panel.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}