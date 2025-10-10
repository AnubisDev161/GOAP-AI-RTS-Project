using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IActionStrategy
{
    bool canPerform { get; }
    bool complete { get; }

    void Start()
    {

    }

    void Update(float deltatime)
    {

    }

    void Stop()
    {

    }
}

