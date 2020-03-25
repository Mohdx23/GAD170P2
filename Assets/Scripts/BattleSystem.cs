using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The BattlesSystem handles the organisation of rounds, selecting the dancers to dance off from each side.
/// It then hands off to the fightManager to determine the outcome of 2 dancers dance off'ing.
/// 
/// TODO:
///     Needs to hand the request for a dance off battle round by selecting a dancer from each side and 
///         handing off to the fight manager, via GameEvents.RequestFight
///     Needs to handle GameEvents.OnFightComplete so that a new round can start
///     Needs to handle a team winning or loosing
///     This may be where characters are set as selected when they are in a dance off and when they leave the dance off
/// </summary>
public class BattleSystem : MonoBehaviour
  //this code is basically essential because it controls the entire system and runs the flow of the dance battle.
{
    public DanceTeam TeamA, TeamB;

    public float battlePrepTime = 2;
    public float fightWinTime = 2;

    private void OnEnable()
    {
        GameEvents.OnRequestFighters += RoundRequested;
        GameEvents.OnFightComplete += FightOver;
    }
    
    private void OnDisable()
    {
        GameEvents.OnRequestFighters -= RoundRequested;
        GameEvents.OnFightComplete -= FightOver;
    }
    //this piece of code above is incharge of organizing how the rounds go and when does the fight finish, it also makes sure a new round starts.

    void RoundRequested()
    {
        //calling the coroutine so we can put waits in for anims to play
        StartCoroutine(DoRound());
    }

    IEnumerator DoRound()
    {
        yield return new WaitForSeconds(battlePrepTime);
        //this code is the most important one in this script since it is incharge of dictating a team victorious when the battle is over.
        //It basically takes both teams and checks the count of them, the team with the more dancers at the end comes out as the winner.
        if (TeamA.activeDancers.Count > 0 && TeamB.activeDancers.Count > 0)
        {
            // Debug.LogWarning("DoRound called, it needs to select a dancer from each team to dance off and put in the FightEventData below");
            Character a = TeamA.activeDancers[Random.Range(0, TeamA.activeDancers.Count)];
            Character b = TeamB.activeDancers[Random.Range(0, TeamB.activeDancers.Count)];

            GameEvents.RequestFight(new FightEventData(a, b));
        }
        else
        {
            DanceTeam winner;

            //winner = TeamA.activeDancers.Count <=0 ? TeamB : TeamA; / You can use this to make if/else statements shorter
            if (TeamA.activeDancers.Count > TeamB.activeDancers.Count)
            {
                winner = TeamA;

            }
            else
                winner = TeamB;

            GameEvents.BattleFinished(winner);
            winner.EnableWinEffects();
            //this code is incharge of displaying the win effects when a dance round is over, also incharge of playing the win effects.



            //log it battlelog also
            //Debug.Log("DoRound called, but we have a winner so Game Over");
        }
    }


    void FightOver(FightResultData data)
    {

        if (data.outcome != 0)
        {
            data.winner.myTeam.EnableWinEffects();

            data.defeated.myTeam.RemoveFromActive(data.defeated);

            //this code is in charge of playing the effects of a victory when the battle is over, it also removes players from the defeated team.
        }
        StartCoroutine(HandleFightOver());
    }

    IEnumerator HandleFightOver()
    {
        yield return new WaitForSeconds(fightWinTime);
        TeamA.DisableWinEffects();
        TeamB.DisableWinEffects();
        //Debug.LogWarning("HandleFightOver called, may need to prepare or clean dancers or teams and checks before doing GameEvents.RequestFighters()");
        GameEvents.RequestFighters();
    }
}
