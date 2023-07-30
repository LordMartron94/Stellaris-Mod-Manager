#region Copyright

//  Stellaris Mod Manager used to manage a library of installed mods for the game of Stellaris
// Copyright (C) 2023  Matthew David van der Hoorn
//
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, at version 3 of the license.
//
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//     You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//
// CONTACT:
// Email: md.vanderhoorn@gmail.com
//     Business Email: admin@studyinstitute.net
// Discord: mr.hoornasp.learningexpert
// Phone: +31 6 18206979

#endregion

using System.ComponentModel;

namespace MD.StellarisModManager.UI.Library.Extensions;

public static class CollectionExtensions
{
    /// <summary>
    /// Adds a range of items to the specified <see cref="BindingList{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The <see cref="BindingList{T}"/> to add items to.</param> 
    /// <param name="items">The collection of items to add.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="list"/> or <paramref name="items"/> is null.
    /// </exception>
    /// <remarks>
    /// This method disables raise list changed events to avoid multiple notifications, 
    /// adds each item from the <paramref name="items"/> collection to the <paramref name="list"/>,
    /// re-enables raise list changed events, and calls <see cref="ResetBindings"/>
    /// to notify binding clients of the updated list.
    /// </remarks>
    public static void AddRange<T>(this BindingList<T> list, IEnumerable<T> items) 
    {
        if (list == null) 
            throw new ArgumentNullException(nameof(list));

        if (items == null)
            throw new ArgumentNullException(nameof(items));

        List<T> source = items.ToList();

        list.RaiseListChangedEvents = false;

        foreach (T item in source)
            list.Add(item);

        list.RaiseListChangedEvents = true; 
        list.ResetBindings();
    }
    
    /// <summary>
    /// Sorts the items in the <see cref="BindingList{T}"/> in ascending order based on the specified key selector function.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <typeparam name="TKey">The type of the key used for sorting.</typeparam>
    /// <param name="source">The <see cref="BindingList{T}"/> to be sorted.</param>
    /// <param name="keySelector">The function used to extract the key for sorting.</param>
    /// <remarks>
    /// This method converts the <paramref name="source"/> <see cref="BindingList{T}"/> to a <see cref="List{T}"/>,
    /// sorts the list in ascending order using the provided <paramref name="keySelector"/> function,
    /// clears the <paramref name="source"/> <see cref="BindingList{T}"/>, and adds the sorted items back to it.
    /// </remarks>
    public static void OrderBy<T, TKey>(this BindingList<T> source, Func<T, TKey> keySelector)
    {
        // Convert the BindingList to a List
        List<T> list = source.ToList();

        // Sort the list using the provided keySelector function
        list.Sort((x, y) => Comparer<TKey>.Default.Compare(keySelector(x), keySelector(y)));
        
        source.Clear();

        // Add the sorted items back to the source BindingList
        foreach (T item in list)
            source.Add(item);
    }

    /// <summary>
    /// Sorts the items in the <see cref="BindingList{T}"/> in descending order based on the specified key selector function.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <typeparam name="TKey">The type of the key used for sorting.</typeparam>
    /// <param name="source">The <see cref="BindingList{T}"/> to be sorted.</param>
    /// <param name="keySelector">The function used to extract the key for sorting.</param>
    /// <remarks>
    /// This method converts the <paramref name="source"/> <see cref="BindingList{T}"/> to a <see cref="List{T}"/>,
    /// sorts the list in descending order using the provided <paramref name="keySelector"/> function,
    /// clears the <paramref name="source"/> <see cref="BindingList{T}"/>, and adds the sorted items back to it.
    /// </remarks>
    public static void OrderByDescending<T, TKey>(this BindingList<T> source, Func<T, TKey> keySelector)
    {
        // Convert the BindingList to a List
        List<T> list = source.ToList();

        // Sort the list in descending order using the provided keySelector function
        list.Sort((x, y) => Comparer<TKey>.Default.Compare(keySelector(y), keySelector(x)));
        
        source.Clear();

        // Add the sorted items back to the source BindingList
        foreach (T item in list)
            source.Add(item);
    }

}