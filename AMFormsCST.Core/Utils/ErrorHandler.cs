using AMFormsCST.Core.Types.Notebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Utils;
internal static class ErrorHandler
{
    internal static void InitMessages(ErrorMessages messages)
    {
        Notes.InitMessages(messages.Notes);
    }
    internal static class Notes
    {
        private static string NoNoteInListMessage = "There is no {0} in {1} that matches the {0}(s) provided.";
        private static string NoteMissingMessage = "{0} is missing:\n{1}";
        private static string BothNotesMissingMessage = "Both notes are missing:\n{0}\n{1}";
        private static string EmptyListMessage = "There are no {0}s in {1} to load.";

        internal static void InitMessages(NotesErrorMessages messages)
        {
            NoNoteInListMessage = messages.NoNoteInListMessage;
            NoteMissingMessage = messages.NoteMissingMessage;
            BothNotesMissingMessage = messages.BothNotesMissingMessage;
            EmptyListMessage = messages.EmptyListMessage;
        }
        internal static void NoteNotFoundErrorCheck(Note noteToFind, IList<Note> listToSearch)
        {
            if (!listToSearch.Contains(noteToFind))
                throw new NullReferenceException(
                    string.Format(NoNoteInListMessage, typeof(Note), nameof(listToSearch)),
                    new NullReferenceException(string.Format(NoteMissingMessage, nameof(noteToFind), noteToFind.Dump())));
        }
        internal static void NotesNotFoundErrorCheck(Note noteToFind1, Note noteToFind2, IList<Note> listToSearch)
        {
            if (!listToSearch.Contains(noteToFind1) || !listToSearch.Contains(noteToFind2))
            {
                var offender = !listToSearch.Contains(noteToFind1) ? noteToFind1 : noteToFind2;
                bool both = !listToSearch.Contains(noteToFind1) && !listToSearch.Contains(noteToFind2);

                throw new NullReferenceException(
                   string.Format(NoNoteInListMessage, typeof(Note), nameof(listToSearch)),
                    new NullReferenceException(
                        both ? string.Format(BothNotesMissingMessage, noteToFind1.Dump(), noteToFind2.Dump()) :
                        offender == noteToFind1 ? string.Format(NoteMissingMessage, nameof(noteToFind1), noteToFind1.Dump()) :
                        string.Format(NoteMissingMessage, nameof(noteToFind2), noteToFind2.Dump())));
            }

        }
        internal static void NoNotesErrorCheck(List<Note> notes)
        {
            if (notes.Count == 0)
                throw new NullReferenceException(string.Format(EmptyListMessage, typeof(Note), nameof(notes)));
        }
    }
}

