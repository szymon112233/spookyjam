using Godot;
using System;

public partial class BlowForce : ShapeCast3D, IBaseSpell
{

    public string Name { get; set; }
    public float ManaCost { get; set; }
    public NPC.HealthStatus HealthStatusEffect { get; set; }
    public SpellType SpellType { get; set; }

    public void SetInitialState(Transform3D trans)
    {
        Transform = trans;
        Rotation = Rotation + new Vector3(float.DegreesToRadians(90), 0, 0);

    }

    public override void _PhysicsProcess(double delta)
    {

        var spaceState = GetWorld3D().DirectSpaceState;
        PhysicsDirectBodyState3D asd = new PhysicsDirectBodyState3DExtension();
        var query = PhysicsRayQueryParameters3D.Create(Transform.Origin, Transform.Origin+-Basis.Z*1000);
        // var query = PhysicsRayQueryParameters3D.Create(Transform.Origin, Basis.Z*1000);
        // GD.Print(ProjectSettings.GetSetting("layer_names/3d_physics/layer_1"));
        // query.CollisionMask = UInt32.MaxValue ^ (1 << 4);
        query.CollisionMask = 0b11111110;
        
        // query.Exclude = [this.GetRid()];
        
        query.CollideWithAreas = true;
        
        // var shapeQuery = new PhysicsShapeQueryParameters3D();
        // Rid rid = PhysicsServer3D.BoxShapeCreate();
        // PhysicsServer3D.ShapeSetData(GetColliderRid(0), shapeQuery);
        
        //shapeQuery.Shape = 
        var result = spaceState.IntersectRay(query);
        //var result2 = spaceState.IntersectShape()
        foreach (var p in result)
        {
            // GD.Print(p);
            
        }
        
        // var query = PhysicsRayQueryParameters3D.Create(GlobalPosition, TargetPosition,
        //     CollisionMask, [GetRid()]);
        // var result = spaceState.IntersectRay(query);

        GD.Print(GetCollisionCount());
        if (GetCollisionCount() > 0)
        {
            for (int i = 0; i < GetCollisionCount(); i++)
            {
                GD.Print(((Node3D)GetCollider(i)).Name);
                if (GetCollider(i) is NPC npc)
                {
                    GD.Print("Was NPC");
                    // npc.ChangeHealthStatus(NPC.HealthStatus.Dead);
                    npc.AddForceAndActivateRagdoll(Transform.Basis.Y);
                }
                else if (GetCollider(i) is PhysicalBone3D bone)
                {
                    if (bone.GetParent().GetParent().GetParent().GetParent() is NPC npc2)
                    {
                        npc2.AddForceAndActivateRagdoll(Transform.Basis.Y);
                    }
                }
            }
        }
        
        QueueFree();
    }


}
