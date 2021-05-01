using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScript : MonoBehaviour
{
    public float _health;
    public float _baseGoblinDamage;
    public float _baseSkeletonDamage;
    public float _miniBossDamageMultiplier;
    public float _bossDamage;
    private Animator _animController;

    void Start() {
      _animController = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "GoblinMinion"){
            _health -= _baseGoblinDamage;
        }
        else if(other.tag == "GoblinArrow"){
            _health -= _baseGoblinDamage;
        }
        else if(other.tag == "GoblinBrute"){
            _health -= _baseGoblinDamage * _miniBossDamageMultiplier;
        }
        else if(other.tag == "SkeletonMinion"){
            _health -= _baseSkeletonDamage;
        }
        else if(other.tag == "SkeletonArrow"){
            _health -= _baseSkeletonDamage;
        }
        else if(other.tag == "SkeletonMageBlast"){
            _health -= _baseSkeletonDamage * 1.3f;
        }
        else if(other.tag == "SkeletonKnight"){
            _health -= _baseSkeletonDamage * _miniBossDamageMultiplier;
        }
        else if(other.tag == "OrcCheiftan"){
            _health -= _bossDamage;
        }

        if(_health <= 0) {
          StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        _animController.CrossFade("Death", 0.2f);
        yield return new WaitForSeconds(2f);
        //Reset
    }
}
