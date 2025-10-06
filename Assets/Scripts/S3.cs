using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implementor: Material
public interface IMaterial
{
    string GetMaterial();
}

public class Copper : IMaterial
{
    public string GetMaterial() => "Đồng";
}

public class Iron : IMaterial
{
    public string GetMaterial() => "Sắt";
}

public class Aluminum : IMaterial
{
    public string GetMaterial() => "Nhôm";
}

// Implementor: Color
public interface IColor
{
    string GetColor();
}

public class Blue : IColor
{
    public string GetColor() => "Xanh";
}

public class Red : IColor
{
    public string GetColor() => "Đỏ";
}

public class Yellow : IColor
{
    public string GetColor() => "Vàng";
}

// Abstraction: Shape
public abstract class Shape
{
    protected IMaterial material;
    protected IColor color;

    protected Shape(IMaterial material, IColor color)
    {
        this.material = material;
        this.color = color;
    }

    public abstract void Display();
}

public class Square : Shape
{
    public Square(IMaterial material, IColor color) : base(material, color) { }

    public override void Display()
    {
        Debug.Log($"Hình vuông, Chất liệu: {material.GetMaterial()}, Màu: {color.GetColor()}");
    }
}

public class Circle : Shape
{
    public Circle(IMaterial material, IColor color) : base(material, color) { }

    public override void Display()
    {
        Debug.Log($"Hình tròn, Chất liệu: {material.GetMaterial()}, Màu: {color.GetColor()}");
    }
}

public class Triangle : Shape
{
    public Triangle(IMaterial material, IColor color) : base(material, color) { }

    public override void Display()
    {
        Debug.Log($"Hình tam giác, Chất liệu: {material.GetMaterial()}, Màu: {color.GetColor()}");
    }
}

public class S3 : MonoBehaviour
{
    void Start()
    {
        Shape square = new Square(new Copper(), new Blue());
        square.Display();

        Shape circle = new Circle(new Iron(), new Red());
        circle.Display();

        Shape triangle = new Triangle(new Aluminum(), new Yellow());
        triangle.Display();
    }
}