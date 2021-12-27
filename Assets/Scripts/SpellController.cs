using System.Collections;using System.Collections.Generic;
using Spells;
using UnityEngine;


/// <summary>
/// Current layout :
/// 1, 2, 3, 4 for water, fire, wind, earth
/// Right mouse button for Casting
///
/// Use the static class "ElementKeys" to change the keyboard layout
///  
/// </summary>
public class SpellController : MonoBehaviour
{
    #region Public Fields
    public float cancelSpellTime = 3;
    public GameObject[] symbols = {};
    public GameObject[] spellInstances = {};
    
    public RadialLayout spinningPanel;
    public Transform player;
    public Transform playerAuraCenter;
    #endregion
    
    private bool _isCountingDown;
    private Transform _transform;
    private readonly List<Symbol> _currentSymbols = new List<Symbol>();
    private readonly List<Symbol>[] _formula = {
        new List<Symbol>{Symbol.Water, Symbol.Water, Symbol.Water, Symbol.Water},   // Basics 
        new List<Symbol>{Symbol.Fire, Symbol.Fire, Symbol.Fire, Symbol.Fire},       // Basics
        new List<Symbol>{Symbol.Wind, Symbol.Wind, Symbol.Wind, Symbol.Wind},       // Basics
        new List<Symbol>{Symbol.Earth, Symbol.Earth, Symbol.Earth, Symbol.Earth},   // Basics
        new List<Symbol>{Symbol.Water, Symbol.Fire, Symbol.Wind, Symbol.Earth},     // Harmony
    };
    private readonly List<SpellType> _spellTypes = new List<SpellType>
    {
        SpellType.Facing,       // Water swirl
        SpellType.Projectile,   // Fire ball
        SpellType.Aura,         // Wind element
        SpellType.Trap,         // Earth trap
        SpellType.Harmony,      // Harmony buff
    };
    
    #region Unity Methods

    private void Start()
    {
        _transform = transform;
    }

    private void Update()
    {
        KeyboardInput();
    }
    
    #endregion

    #region Private Methods

    private void KeyboardInput()
    {
        // Save resources
        if (!Input.anyKey) return;

        // Refresh SpellCD to X seconds if any of element-key is pressed
        RefreshSpellCountDown();
        
        // Create element according to the key pressed
        CreateElement();
        
        // Cast Spell if pressed cast-key
        CastSpell();
    }

    private IEnumerator CancelSpelling()
    {
        _isCountingDown = true;

        while (cancelSpellTime > 0)
        {
            cancelSpellTime -= Time.deltaTime;
            yield return null;
        }

        // Remove all child in Spell Panel if timeout
        int childCount = spinningPanel.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
            Destroy(spinningPanel.transform.GetChild(i).gameObject);
        
        // Clear current symbol list
        _currentSymbols.Clear();
        
        _isCountingDown = false;
    }

    private void RefreshSpellCountDown()
    {
        if (
            (Input.GetKeyDown(ElementKeys.KeyCodeWater)) ||
            (Input.GetKeyDown(ElementKeys.KeyCodeFire))  ||
            (Input.GetKeyDown(ElementKeys.KeyCodeWind))  ||
            (Input.GetKeyDown(ElementKeys.KeyCodeEarth))
        ){
            cancelSpellTime = 3;
            if (!_isCountingDown)
                StartCoroutine(CancelSpelling());
        }
    }

    private void CreateElement()
    {
        // WATER, FIRE, WIND, EARTH elements
        if (Input.GetKeyDown(ElementKeys.KeyCodeWater))
        {
            Instantiate(symbols[(int) Symbol.Water], Vector3.zero, Quaternion.identity, spinningPanel.transform);
            _currentSymbols.Add(Symbol.Water);
        }
        if (Input.GetKeyDown(ElementKeys.KeyCodeFire))
        {
            Instantiate(symbols[(int) Symbol.Fire], Vector3.zero, Quaternion.identity, spinningPanel.transform);
            _currentSymbols.Add(Symbol.Fire);
        }
        if (Input.GetKeyDown(ElementKeys.KeyCodeWind))
        {
            Instantiate(symbols[(int) Symbol.Wind], Vector3.zero, Quaternion.identity, spinningPanel.transform);
            _currentSymbols.Add(Symbol.Wind);
        }
        if (Input.GetKeyDown(ElementKeys.KeyCodeEarth))
        {
            Instantiate(symbols[(int) Symbol.Earth], Vector3.zero, Quaternion.identity, spinningPanel.transform);
            _currentSymbols.Add(Symbol.Earth);
        }
    }

    private void CastSpell()
    {
        if (Input.GetKeyDown(ElementKeys.KeyCodeCast))
        {
            int len = _formula.Length;
            for (int i = 0; i < len; i++)
            {
                // Check if current element set is correct
                if ( CheckMatch(_currentSymbols, _formula[i]) )
                {
                    GameObject instance;
                    // Spawn spell instance here
                    // The position of the spellInstance will affect where it spawns
                    // Remember to reset the position of those prefabs if you don't want to offset them by accident
                    if (_spellTypes[i] == SpellType.Aura)
                    {
                        // Spawn aura type spell under the aura center, so no need to use player's position
                        instance = Instantiate(
                            spellInstances[i], 
                            spellInstances[i].transform.position, 
                            Quaternion.identity,
                            playerAuraCenter
                        );
                    }
                    else if (_spellTypes[i] == SpellType.Facing)
                    {
                        // Spawn facing type spell under player, so no need to use player's position
                        // ????????????????????????
                        instance = Instantiate(
                            spellInstances[i], 
                            player.position, 
                            player.rotation,
                            player
                        );
                    }
                    else if (_spellTypes[i] == SpellType.Harmony)
                    {
                        // Harmony type spell
                        // Spawn particle prefab under Spell Controller, which is child of player
                        instance = Instantiate(
                            spellInstances[i], 
                            _transform.position, 
                            _transform.rotation,
                            _transform
                        );
                    }
                    else
                    {
                        // Spawn other spells at player's position, and follow player's rotation 
                        instance = Instantiate(
                            spellInstances[i], 
                            player.position + spellInstances[i].transform.position, 
                            player.rotation
                        );
                    }

                    // Harmony -> Buff spells
                    if (_transform.childCount > 0 && _spellTypes[i] != SpellType.Harmony)
                    {
                        // Remove effect
                        Destroy(_transform.GetChild(0).gameObject);
                        // Call buff function
                        instance.GetComponent<ISpellHarmony>().Harmony();
                    }
                    
                    break;
                }
            }
            
            // Reuse the code from the cancel method 
            cancelSpellTime = -1;
        }
    }
    #endregion

    #region Utilities

    bool CheckMatch(IReadOnlyList<Symbol> l1, IReadOnlyList<Symbol> l2)
    {
        if (l1 == null && l2 == null)
            return true;
        if (l1 == null || l2 == null)
            return false;
        if (l1.Count != l2.Count)
            return false;
        int len = l1.Count;
        for (int i = 0; i < len; i++) {
            if (l1[i] != l2[i])
                return false;
        }
        return true;
    }

    #endregion
}

public enum Symbol
{
    Water, 
    Fire, 
    Wind,
    Earth
}

public enum SpellType
{
    None,
    Projectile,
    Trap,
    Aura,
    Facing, 
    Harmony
}

public static class ElementKeys
{
    public const KeyCode KeyCodeWater = KeyCode.Alpha1;
    public const KeyCode KeyCodeFire = KeyCode.Alpha2;
    public const KeyCode KeyCodeWind = KeyCode.Alpha3;
    public const KeyCode KeyCodeEarth = KeyCode.Alpha4;
    public const KeyCode KeyCodeCast = KeyCode.Mouse1;
}