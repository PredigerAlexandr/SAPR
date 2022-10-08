using ConsoleApp1.Constant;
using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ConsoleApp1
{
    internal class Analysis
    {
        List<Lexem> lexems = new List<Lexem>();
        List<Variable> variables = new List<Variable>();
        StreamReader reader;
        State state = State.Initial;
        int countString = 1;
        int idVar = 0;
        int idConstant = 0;
        string buffer = "";
        string followBuffer = "";
        string nextBufer = "";
        char[] singleBuffer = new char[1];
        bool final = false;


        public Tuple<List<Lexem>, List<Variable>> AnalysisProcess(string path)
        {
            reader = new StreamReader(path);
            while(state != State.Final && state != State.Error)
            {
                switch (state)
                {
                    case State.Initial:
                        if (final)
                        {
                            state = State.Final;
                            break;
                        }
                        if (reader.Peek() == -1)
                        {
                            final = true;
                        }
                        
                        if (EmptyOrNoVisible(singleBuffer[0])){
                            NextCharElem();
                        }
                        else if (IsLetter(singleBuffer[0])){
                            ClearBuffers();
                            AddElementToBuffer(singleBuffer[0]);
                            state = State.ReadingStr;
                            NextCharElem();
                        }
                        else if (IsNumber(singleBuffer[0])){
                            ClearBuffers();
                            AddElementToBuffer(singleBuffer[0]);
                            state = State.ReadingNum;
                            NextCharElem();
                        }
                        else
                        {
                            ClearBuffers();
                            AddElementToBuffer(singleBuffer[0]);
                            state = State.ReadDelOrOper;
                            NextCharElem();
                        }
                        break;

                    case State.ReadingStr:
                        if (IsLetter(singleBuffer[0]) || IsNumber(singleBuffer[0]))
                        {
                            AddElementToBuffer(singleBuffer[0]);
                            NextCharElem();
                        }
                        else if (EmptyOrNoVisible(singleBuffer[0]))
                        {
                            var searchLexem = Constants.Keywords.Contains(buffer);
                            var searchType = Constants.Types.ContainsKey(buffer);
                            var searhVariables = variables.FirstOrDefault(v => v.Name == buffer);

                            if (searchLexem)
                            {
                                lexems.Add(new Lexem(LexemType.Identifier, Array.IndexOf(Constants.Keywords, buffer), buffer));
                                ClearBuffers();
                                state = State.Initial;
                                NextCharElem();
                            } else if (searchType)
                            {
                                lexems.Add(new Lexem(LexemType.TypeVar, Constants.Types[buffer].Item1, Constants.Types[buffer].Item2));
                                ClearBuffers();
                                state = State.Initial;
                                NextCharElem();
                            }else if (searhVariables!=null)
                            {
                                lexems.Add(new Lexem(LexemType.Constant, searhVariables.Id, $"variable <{searhVariables.Name}>"));
                                ClearBuffers();
                                state = State.Initial;
                                NextCharElem();
                            }
                            else
                            {
                                var lastDataTypeItem = lexems.LastOrDefault(l => l.LexemType == LexemType.TypeVar);
                                if (lastDataTypeItem != null)
                                {
                                    variables.Add(new Variable(idVar++, lastDataTypeItem.Value, buffer));
                                    ClearBuffers();
                                    state = State.Initial;
                                    NextCharElem();
                                }
                            }
                        }
                        break;

                    case State.ReadingNum:
                        if (IsLetter(singleBuffer[0]))
                        {
                            AddElementToBuffer(singleBuffer[0]);
                            state = State.ReadingStr;
                            NextCharElem();
                        }
                        else if (IsNumber(singleBuffer[0]))
                        {
                            AddElementToBuffer(singleBuffer[0]);
                            state = State.ReadingNum;
                            NextCharElem();
                        }
                        else if (EmptyOrNoVisible(singleBuffer[0]))
                        {
                            var lastItemDataType = lexems.LastOrDefault(l => l.LexemType == LexemType.TypeVar);
                            
                            if(lastItemDataType != null)
                            {
                                lexems.Add(new Lexem(LexemType.Constant, idConstant++, $"Numeric with value = {buffer}"));
                                ClearBuffers();
                                state = State.Initial;
                                NextCharElem();
                            }
                            else
                            {
                                state = State.Error;
                            }
                        }
                        else
                        {
                            lexems.Add(new Lexem(LexemType.Constant, idConstant++, $"Numeric with value = {buffer}"));
                            ClearBuffers();
                            state = State.Initial;
                        }
                        break;

                    case State.ReadDelOrOper:
                        var searchDelmetr = Constants.KeySymbols.Contains(buffer);
                        if (searchDelmetr)
                        {
                            lexems.Add(new Lexem(LexemType.Delimetr, Array.IndexOf(Constants.KeySymbols, buffer), buffer));
                            ClearBuffers();
                            if (EmptyOrNoVisible(singleBuffer[0]))
                                NextCharElem();
                            state = State.Initial;
                        }else if (EmptyOrNoVisible(singleBuffer[0]) || IsLetter(singleBuffer[0]) || IsNumber(singleBuffer[0]))
                        {
                            var searchOperation = Constants.Operations.ContainsKey(buffer);

                            if (searchOperation)
                            {
                                lexems.Add(new Lexem(LexemType.Operation, Constants.Operations[buffer].Item1, Constants.Operations[buffer].Item2));
                                ClearBuffers();
                                NextCharElem();
                                state = State.Initial;
                            }
                            else
                            {
                                state = State.Error;
                            }
                        }
                        else
                        {
                            AddElementToBuffer(singleBuffer[0]);
                            NextCharElem();
                        }
                        break;

                    case State.Error:
                        variables.Add(new Variable(0, "Error", $"Error on {countString} line "));
                        state = State.Final;
                        break;

                    case State.Final:
                        return new Tuple<List<Lexem>, List<Variable>>(lexems, variables);
                }
            }
            return new Tuple<List<Lexem>, List<Variable>>(lexems,variables);
        }

        private bool EmptyOrNoVisible(char item)//проверяем на пробел или другой разделительный элемент или если singleBuffer только что иницилизированный
        {
            if (item == '\n')
                countString++;
            if (item == ' ' || item == '\0' || item == '\n' || item == '\t' || item == '\r')
                return true;
            else
                return false;
        }

        private void NextCharElem()//сдвигаемся на один элемент
        {
            reader.Read(singleBuffer, 0, 1);

        }
        private bool IsLetter(char item)//проверка на то, является ли элемент строкой
        {
            return Char.IsLetter(item);
        }
        private void ClearBuffers()//очистка буфферов 
        {
            buffer = "";
            followBuffer = "";
        }
        private void AddElementToBuffer(char item)//добавляем элемент в буффер
        {
            buffer += item;
        }
        private bool IsNumber(char iter)
        {
            if (iter == '-')
            {
                return true;
            }
            return Char.IsDigit(iter);
        }

    }
}
