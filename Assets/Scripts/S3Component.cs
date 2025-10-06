using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Move States
public interface IMoveState
{
    string GetState();
}

public class IdleState : IMoveState
{
    public string GetState() => "Idle";
}

public class RunState : IMoveState
{
    public string GetState() => "Run";
}

public class JumpState : IMoveState
{
    public string GetState() => "Jump";
}

public class FallingState : IMoveState
{
    public string GetState() => "Falling";
}

// Move Component
public class MoveComponent
{
    private IMoveState moveState;

    public MoveComponent(IMoveState state)
    {
        moveState = state;
    }

    public void SetState(IMoveState state)
    {
        moveState = state;
    }

    public void Display()
    {
        Debug.Log($"Move State: {moveState.GetState()}");
    }
}

// Attack States
public interface IAttackState
{
    string GetState();
}

public class Skill1State : IAttackState
{
    public string GetState() => "Skill 1";
}

public class Skill2State : IAttackState
{
    public string GetState() => "Skill 2";
}

// Attack Component
public class AttackComponent
{
    private IAttackState attackState;

    public AttackComponent(IAttackState state)
    {
        attackState = state;
    }

    public void SetState(IAttackState state)
    {
        attackState = state;
    }

    public void Display()
    {
        Debug.Log($"Attack State: {attackState.GetState()}");
    }
}

// Health Component
public class HealthComponent
{
    public int Health { get; private set; }

    public HealthComponent(int health)
    {
        Health = health;
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health < 0) Health = 0;
    }

    public void Display()
    {
        Debug.Log($"Health: {Health}");
    }
}

// Character
public class Character
{
    public MoveComponent Move { get; private set; }
    public AttackComponent Attack { get; private set; }
    public HealthComponent Health { get; private set; }

    public Character(MoveComponent move, AttackComponent attack, HealthComponent health)
    {
        Move = move;
        Attack = attack;
        Health = health;
    }

    public void Display()
    {
        Move.Display();
        Attack.Display();
        Health.Display();
    }
}

public class S3Component : MonoBehaviour
{
    void Start()
    {
        var move = new MoveComponent(new IdleState());
        var attack = new AttackComponent(new Skill1State());
        var health = new HealthComponent(100);

        var character = new Character(move, attack, health);
        character.Display();

        // Chuyển trạng thái
        move.SetState(new RunState());
        attack.SetState(new Skill2State());
        health.TakeDamage(20);

        character.Display();
    }
}