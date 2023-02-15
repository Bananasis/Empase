using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Cell))]
[RequireComponent(typeof(Collider2D))]
public class CellCollider : MonoBehaviour
{
    [Inject, SerializeField, HideInInspector]
    private CellManager _gCellData;

    private Cell _cell;


    private void Awake()
    {
        _cell = GetComponent<Cell>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var colCell = other.GetComponent<Cell>();
        if (colCell == null || colCell.cData.type == CellType.GoTo) return;
        Collide(colCell);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var colCell = other.GetComponent<Cell>();
        if (colCell == null || colCell.cData.type == CellType.GoTo) return;
        Collide(colCell);
    }

    // public void Rebounce(Cell cell)
    // {
    //     if (cell.mass == 0) return;
    //
    //     // if (mass < cell.mass) return;
    //     if (id < cell.id) return;
    //     var vSelf = velocity;
    //     var vCell = cell.velocity;
    //
    //
    //     var vec = position - cell.position;
    //     var dist = vec.magnitude;
    //     var dir = vec.normalized;
    //     var intersection = size + cell.size - dist;
    //
    //     if (intersection < 0)
    //     {
    //         Debug.DrawLine(position,cell.position);
    //         Debug.DrawLine(position,Vector3.back);
    //         Debug.LogError($"Impossible Collision: {intersection}");
    //         return;
    //     }
    //     var selfIM = mass * inertiaMultiplier;
    //     var cellIM = cell.mass * cell.inertiaMultiplier;
    //
    //     var selfEIM = selfIM * hardness; //- Mathf.Pow(-Mathf.Min(intersection, size) + size, 2) * Mathf.PI;
    //     var cellEIM = cellIM * hardness; //- Mathf.Pow(-Mathf.Min(intersection, cell.size) + cell.size, 2) * Mathf.PI;
    //
    //     var eMRationSelf = selfEIM / selfIM;
    //     var eMRationCell = cellEIM / cellIM;
    //
    //     var pVelSelf = Vector2.Dot(vSelf, dir) * dir;
    //     var pVelCell = Vector2.Dot(vCell, dir) * dir;
    //
    //
    //     var pMomSelf = pVelSelf * selfEIM;
    //     var pMomCell = pVelCell * cellEIM;
    //
    //     var dVelSelf = pinned ? Vector3.zero : (pMomCell - pMomSelf) / selfIM;
    //     var dVelCell = cell.pinned ? Vector3.zero : (pMomSelf - pMomCell) / cellIM;
    //
    //     
    //
    //     velocity += dVelSelf;
    //     cell.velocity += dVelCell;
    //
    //     var momentumTwerk = (velocity* selfIM + cell.velocity * cellIM -
    //                         (vSelf * selfIM + vCell * cellIM)).magnitude;
    //     if (Math.Abs(momentumTwerk) > 0.01)
    //     {
    //         DrawCollisionDebug(position,vSelf,velocity , selfIM);
    //         DrawCollisionDebug(cell.position,vCell,cell.velocity,cellIM);
    //         Debug.LogError($"Momentum Twerk: {momentumTwerk}");
    //     }
    //
    //     var energyChange = (velocity.sqrMagnitude * selfIM + cell.velocity.sqrMagnitude * cellIM) -
    //                      (vSelf.sqrMagnitude * selfIM + vCell.sqrMagnitude * cellIM);
    //     if (energyChange > 0.0)
    //     {
    //         DrawCollisionDebug(position,vSelf,velocity , selfIM);
    //         DrawCollisionDebug(cell.position,vCell,cell.velocity,cellIM);
    //         Debug.LogError($"Energy spike: {energyChange}");
    //     }
    //         
    //     Debug.Log($"Before:{vSelf} {vCell} \n" +
    //               $"After:{velocity} {cell.velocity}\n" +
    //               $"Mass: {mass} {cell.mass}\n" +
    //               $"EMass: {selfEIM} {cellEIM}\n" +
    //               $"EMassRatio: {eMRationSelf} {eMRationCell}\n" +
    //               $"Intersection: {intersection}\n" +
    //               $"Size: {size} {cell.size}\n" +
    //               $"MomentumTransfer: {pMomSelf} {pMomCell}"
    //     );
    //     
    //    
    //    return;
    //
    //     var escapeVel = dir ;
    //     if (escapeVel.magnitude == 0) return;
    //     var escapeTime = intersection / escapeVel.magnitude;
    //     transform.position += (Vector3) dir*Mathf.Abs(cell.mass)/ (Mathf.Abs(mass) + Mathf.Abs(cell.mass)) * escapeTime;
    //     cell.transform.position -= (Vector3)dir *Mathf.Abs(mass)/ (Mathf.Abs(mass) + Mathf.Abs(cell.mass)) * escapeTime;
    // }

    private void DrawCollisionDebug(Vector2 pos, Vector2 iVelocity, Vector2 rVelocity, float mass)
    {
        Debug.DrawLine(pos, pos + rVelocity - iVelocity, new Color(1, 0.5f, 0), Time.unscaledDeltaTime);
        Debug.DrawLine(pos + Vector2.up, pos + Vector2.up + rVelocity * mass - iVelocity * mass, new Color(0.5f, 1, 0),
            Time.deltaTime);
        Debug.DrawLine(pos, pos + Vector2.up * (rVelocity.sqrMagnitude - iVelocity.sqrMagnitude) * mass,
            new Color(0f, 0, 1), Time.deltaTime);

        return;

        Debug.DrawLine(pos + Vector2.up, pos + Vector2.up + rVelocity * mass, new Color(0.6f, 0.6f, 0), Time.deltaTime);
        Debug.DrawLine(pos + Vector2.up, pos + Vector2.up + iVelocity * mass, new Color(0.5f, 0.1f, 0.1f),
            Time.deltaTime);
        Debug.DrawLine(pos, pos + rVelocity, Color.red, Time.deltaTime);
        Debug.DrawLine(pos, pos + iVelocity, Color.yellow, Time.deltaTime);
        Debug.DrawLine(pos + Vector2.down, pos + Vector2.down + rVelocity * rVelocity.magnitude * mass,
            new Color(0.5f, 1, 1), Time.deltaTime);
        Debug.DrawLine(pos + Vector2.down, pos + Vector2.down + iVelocity * iVelocity.magnitude * mass,
            new Color(0f, 0.5f, 1), Time.deltaTime);
    }

    public virtual void Collidelegacy(Cell cell)
    {
        var self = _cell.cData;
        var other = cell.cData;
        var sMass = self.mass;
        var oMass = other.mass;
        if (oMass == 0 || sMass == 0) return;
        if (oMass > sMass) return;
        if (self.massMultiplier * other.massMultiplier < 0)
        {
            CollideNegativeMass(cell, sMass, oMass);
            //Rebounce(cell);
            return;
        }

        var distance = (self.position - other.position).magnitude;
        var intersection = self.size + other.size - distance;
        if (intersection < 0) return;
        var tMass = oMass + sMass;
        var delta = Mathf.Sqrt(tMass / (2 * Mathf.PI) - distance * distance / 4);
        var r1 = distance / 2 + delta;
        if (r1 > distance)
        {
            _cell.SetMass(tMass);
            cell.Die();
        }
        else
        {
            _cell.SetSize(r1);
            cell.SetSize(distance - r1);
        }

        var newMass = _cell.cData.mass;
        var deltaMass = newMass - sMass;
        if (!self.pinned)
            _cell.cData.velocity = (self.velocity * sMass +
                                    other.velocity * deltaMass * self.inertiaMultiplier * other.inertiaMultiplier) /
                                   newMass;
    }

    public virtual void Collide(Cell cell)
    {
        var self = _cell.cData;
        var other = cell.cData;
        var sMass = self.mass;
        var oMass = other.mass;
        if (oMass == 0 || sMass == 0) return;
        if (oMass > sMass) return;
        if (self.massMultiplier * other.massMultiplier < 0)
        {
            CollideNegativeMass(cell, sMass, oMass);
            //Rebounce(cell);
            return;
        }

        var distance = (self.position - other.position).magnitude;
        var intersection = self.size + other.size - distance;
        if (intersection < 0) return;

        var massDefect = _gCellData.massDefect;
        var tMass = oMass * massDefect + sMass;
        var r1 = CalculateCollision(distance, massDefect, tMass);
        if (r1 > distance)
        {
            _cell.SetMass(tMass); //todo
            _cell.OnConsume.Invoke(cell);
            cell.OnConsumed.Invoke(_cell);
            cell.Die();
        }
        else
        {
            _cell.SetSize(r1);
            cell.SetSize(distance - r1);
        }

        if (self.pinned) return;
        var newMass = _cell.cData.mass;
        var deltaMass = newMass - sMass;
        _cell.cData.velocity = (self.velocity * sMass +
                                other.velocity * deltaMass * self.inertiaMultiplier * other.inertiaMultiplier) /
                               newMass;
    }

    public float CalculateCollision(float dist, float masDefect, float mass)
    {
        var gap = Mathf.Sqrt(mass * (masDefect + 1) / (Mathf.PI * masDefect));
        return (Mathf.Sqrt(mass * (masDefect + 1) / Mathf.PI - dist * dist * masDefect) + dist * masDefect) /
               (masDefect + 1);
    }


    public virtual void CollideNegativeMass(Cell cell, float sMass, float oMass)
    {
        var self = _cell.cData;
        var other = cell.cData;
        var cMass = sMass - oMass;
        var distance = (self.position - other.position).magnitude;
        var intersection = self.size + other.size - distance;
        if (intersection < 0) return;

        var r1 = (cMass / Mathf.PI + distance * distance) / (2 * distance);
        if (r1 > distance)
        {
            _cell.SetMass(cMass);
            _cell.OnConsume.Invoke(cell);
            cell.OnConsumed.Invoke(_cell);
            cell.Die();
        }
        else
        {
            _cell.SetSize(r1);
            cell.SetSize(distance - r1);
        }
    }
}