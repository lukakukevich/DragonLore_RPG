using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{

    

    public class Fighter : MonoBehaviour, IAction
    {
        Health target;
        float timeSinceLastAttack = 0;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;

        private void Update()
        {

            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;


            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
                
            }
            else
            {
                GetComponent<Mover>().Cancel();
                
                AttackBehaviour();
                transform.LookAt(target.transform);
            }

        }

        private void AttackBehaviour()
        {

            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // this will trigger Hit() 
                TriggerAttack();
                timeSinceLastAttack = 0;

            }


        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }



        //Animation Event
        void Hit()
        {
            if (target == null) { return; }
            target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionSchedule>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }


            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        


    }



}
