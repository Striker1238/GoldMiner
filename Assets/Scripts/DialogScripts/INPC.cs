using System.Collections.Generic;

public interface INPC
{
    string Name { get; }
    NPCType Type { get; }
    List<DialogueLine> Dialogue { get; }
}