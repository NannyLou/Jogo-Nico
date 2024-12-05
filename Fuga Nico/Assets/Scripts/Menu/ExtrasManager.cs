using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Importar TextMeshPro

public class ExtrasManager : MonoBehaviour
{
    [Header("Configurações de UI")]
    public GameObject extrasPanel;
    public GameObject mainMenuPanel;
    public GameObject characterDetailPanel;
    public Button backButtonExtras;
    public Button backButtonDetail;
    
    [Header("Configurações de Personagem")]
    public GameObject characterButtonPrefab;
    public Transform contentParent;

    [Header("Detalhes do Personagem")]
    public Image characterImage;
    public TextMeshProUGUI characterNameText; // Atualizado para TextMeshProUGUI
    public TextMeshProUGUI characterInfoText; // Atualizado para TextMeshProUGUI

    [Header("Lista de Personagens")]
    public List<GameCharacter> characters; // Atualizado para GameCharacter

    private void Start()
    {
        // Inicialmente, apenas o MainMenuPanel está ativo
        ShowPanel(mainMenuPanel);
        HidePanel(extrasPanel);
        HidePanel(characterDetailPanel);

        // Adicionar listeners aos botões de voltar
        backButtonExtras.onClick.AddListener(ShowMainMenu);
        backButtonDetail.onClick.AddListener(ShowExtrasPanel);

        // Popula a lista de personagens na página de Extras
        PopulateExtras();
    }

    private void PopulateExtras()
    {
        Debug.Log("Populando a lista de personagens...");
        foreach (GameCharacter character in characters)
        {
            GameObject buttonObj = Instantiate(characterButtonPrefab, contentParent);
            Button button = buttonObj.GetComponent<Button>();
            Image buttonImage = buttonObj.GetComponent<Image>();
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>(); // Atualizado para TextMeshProUGUI

            // Atribuir o sprite e o nome
            if (buttonImage != null && character.characterSprite != null)
            {
                buttonImage.sprite = character.characterSprite;
                Debug.Log($"Atribuído sprite para {character.characterName}");
            }
            else
            {
                Debug.LogWarning($"Sprite não atribuído para {character.characterName}");
            }

            if (buttonText != null)
            {
                buttonText.text = character.characterName;
                Debug.Log($"Atribuído nome para {character.characterName}");
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI não encontrado no prefab do botão.");
            }

            // Adicionar listener ao botão
            button.onClick.AddListener(() => ShowCharacterDetail(character));
        }
    }

    public void ShowExtrasPanel()
    {
        Debug.Log("ExtrasPanel ativado.");
        ShowPanel(extrasPanel);
    }

    public void ShowMainMenu()
    {
        Debug.Log("MainMenuPanel ativado.");
        ShowPanel(mainMenuPanel);
    }

    public void ShowCharacterDetail(GameCharacter character)
    {
        Debug.Log($"CharacterDetailPanel ativado para: {character.characterName}");
        ShowPanel(characterDetailPanel);

        // Atribuir detalhes do personagem
        if (characterImage != null && character.characterSprite != null)
        {
            characterImage.sprite = character.characterSprite;
            Debug.Log($"Atribuído sprite detalhado para {character.characterName}");
        }

        if (characterNameText != null)
        {
            characterNameText.text = character.characterName;
            Debug.Log($"Atribuído nome detalhado: {character.characterName}");
        }

        if (characterInfoText != null)
        {
            characterInfoText.text = character.characterInfo;
            Debug.Log($"Atribuído info detalhado para {character.characterName}");
        }
    }

    // Método auxiliar para mostrar apenas um painel
    void ShowPanel(GameObject panelToShow)
    {
        mainMenuPanel.SetActive(panelToShow == mainMenuPanel);
        extrasPanel.SetActive(panelToShow == extrasPanel);
        characterDetailPanel.SetActive(panelToShow == characterDetailPanel);
    }

    // Método auxiliar para esconder um painel
    void HidePanel(GameObject panelToHide)
    {
        panelToHide.SetActive(false);
    }
}

// Classe para armazenar dados dos personagens
[System.Serializable]
public class GameCharacter
{
    public string characterName;
    public Sprite characterSprite;
    [TextArea]
    public string characterInfo;
}
