-Basic usage: 
    -Choose "GameObject -> Procedutal Primitives -> your primitive" to create a new primitive.
    -Adjust primitive's parameters in inspector.

-Scripting:
    -Use "ProceduralPrimitives.CreatePrimitive(primitiveType)" function to create a new primitive.
    -Adjust primitive's parameters and call "Apply()" function to apply changes.
    -example:
        Sphere m_sphere = ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.Sphere).GetComponent<Sphere>();
        m_sphere.radius = 1.5f;
        m_sphere.sliceOn = true;
        m_sphere.sliceFrom = 90.0f;
        m_sphere.Apply();
        Mesh getSphereMesh = m_sphere.mesh; //to get the sphere mesh