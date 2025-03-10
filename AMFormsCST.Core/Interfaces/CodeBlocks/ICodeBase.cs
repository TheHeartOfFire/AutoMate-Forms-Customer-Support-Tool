﻿using AMFormsCST.Core.Types.CodeBlocks;

namespace AMFormsCST.Core.Interfaces.CodeBlocks;

public interface ICodeBase
{
    string? Description { get; set; }
    List<CodeInput> Inputs { get; }
    string? Name { get; set; }
    string? Prefix { get; set; }

    CodeBase AddInput(CodeBase value, string description);
    CodeBase AddInput(CodeInput input);
    CodeBase AddInput(int index, string description);
    CodeBase AddInput(string description);
    CodeBase AddInput(string value, string description);
    string GetCode();
    object GetDescription(int idx);
    object GetInput(int idx);
    List<CodeInput> GetInputs();
    bool HasNoInputs();
    int InputCount();
    CodeBase RemoveInput(int index);
    CodeBase SetInputDescription(int index, string value);
    CodeBase SetInputs(List<string> inputs);
    CodeBase SetInputValue(int index, CodeBase value);
    CodeBase SetInputValue(int index, string value);
}