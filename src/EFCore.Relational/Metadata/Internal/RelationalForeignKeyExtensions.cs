// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal
{
    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public static class RelationalForeignKeyExtensions
    {
        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public static bool AreCompatible(
            [NotNull] this IForeignKey foreignKey,
            [NotNull] IForeignKey duplicateForeignKey,
            in StoreObjectIdentifier storeObject,
            bool shouldThrow)
        {
            var principalType = foreignKey.PrincipalEntityType;
            var duplicatePrincipalType = duplicateForeignKey.PrincipalEntityType;
            if (!string.Equals(principalType.GetSchema(), duplicatePrincipalType.GetSchema(), StringComparison.OrdinalIgnoreCase)
                || !string.Equals(principalType.GetTableName(), duplicatePrincipalType.GetTableName(), StringComparison.OrdinalIgnoreCase))
            {
                if (shouldThrow)
                {
                    throw new InvalidOperationException(
                        RelationalStrings.DuplicateForeignKeyPrincipalTableMismatch(
                            foreignKey.Properties.Format(),
                            foreignKey.DeclaringEntityType.DisplayName(),
                            duplicateForeignKey.Properties.Format(),
                            duplicateForeignKey.DeclaringEntityType.DisplayName(),
                            foreignKey.DeclaringEntityType.GetSchemaQualifiedTableName(),
                            foreignKey.GetConstraintName(storeObject,
                                StoreObjectIdentifier.Table(foreignKey.PrincipalEntityType.GetTableName(),
                                foreignKey.PrincipalEntityType.GetSchema())),
                            principalType.GetSchemaQualifiedTableName(),
                            duplicatePrincipalType.GetSchemaQualifiedTableName()));
                }

                return false;
            }

            if (!SameColumnNames(foreignKey.Properties, duplicateForeignKey.Properties, storeObject))
            {
                if (shouldThrow)
                {
                    throw new InvalidOperationException(
                        RelationalStrings.DuplicateForeignKeyColumnMismatch(
                            foreignKey.Properties.Format(),
                            foreignKey.DeclaringEntityType.DisplayName(),
                            duplicateForeignKey.Properties.Format(),
                            duplicateForeignKey.DeclaringEntityType.DisplayName(),
                            foreignKey.DeclaringEntityType.GetSchemaQualifiedTableName(),
                            foreignKey.GetConstraintName(storeObject,
                                StoreObjectIdentifier.Table(foreignKey.PrincipalEntityType.GetTableName(),
                                foreignKey.PrincipalEntityType.GetSchema())),
                            foreignKey.Properties.FormatColumns(storeObject),
                            duplicateForeignKey.Properties.FormatColumns(storeObject)));
                }

                return false;
            }

            if (!SameColumnNames(foreignKey.PrincipalKey.Properties, duplicateForeignKey.PrincipalKey.Properties, storeObject))
            {
                if (shouldThrow)
                {
                    throw new InvalidOperationException(
                        RelationalStrings.DuplicateForeignKeyPrincipalColumnMismatch(
                            foreignKey.Properties.Format(),
                            foreignKey.DeclaringEntityType.DisplayName(),
                            duplicateForeignKey.Properties.Format(),
                            duplicateForeignKey.DeclaringEntityType.DisplayName(),
                            foreignKey.DeclaringEntityType.GetSchemaQualifiedTableName(),
                            foreignKey.GetConstraintName(storeObject,
                                StoreObjectIdentifier.Table(foreignKey.PrincipalEntityType.GetTableName(),
                                foreignKey.PrincipalEntityType.GetSchema())),
                            foreignKey.PrincipalKey.Properties.FormatColumns(storeObject),
                            duplicateForeignKey.PrincipalKey.Properties.FormatColumns(storeObject)));
                }

                return false;
            }

            if (foreignKey.IsUnique != duplicateForeignKey.IsUnique)
            {
                if (shouldThrow)
                {
                    throw new InvalidOperationException(
                        RelationalStrings.DuplicateForeignKeyUniquenessMismatch(
                            foreignKey.Properties.Format(),
                            foreignKey.DeclaringEntityType.DisplayName(),
                            duplicateForeignKey.Properties.Format(),
                            duplicateForeignKey.DeclaringEntityType.DisplayName(),
                            foreignKey.DeclaringEntityType.GetSchemaQualifiedTableName(),
                            foreignKey.GetConstraintName(storeObject,
                                StoreObjectIdentifier.Table(foreignKey.PrincipalEntityType.GetTableName(),
                                foreignKey.PrincipalEntityType.GetSchema()))));
                }

                return false;
            }

            if (foreignKey.DeleteBehavior != duplicateForeignKey.DeleteBehavior)
            {
                if (shouldThrow)
                {
                    throw new InvalidOperationException(
                        RelationalStrings.DuplicateForeignKeyDeleteBehaviorMismatch(
                            foreignKey.Properties.Format(),
                            foreignKey.DeclaringEntityType.DisplayName(),
                            duplicateForeignKey.Properties.Format(),
                            duplicateForeignKey.DeclaringEntityType.DisplayName(),
                            foreignKey.DeclaringEntityType.GetSchemaQualifiedTableName(),
                            foreignKey.GetConstraintName(storeObject,
                                StoreObjectIdentifier.Table(foreignKey.PrincipalEntityType.GetTableName(),
                                foreignKey.PrincipalEntityType.GetSchema())),
                            foreignKey.DeleteBehavior,
                            duplicateForeignKey.DeleteBehavior));
                }

                return false;
            }

            return true;

            static bool SameColumnNames(
                IReadOnlyList<IProperty> properties,
                IReadOnlyList<IProperty> duplicateProperties,
                in StoreObjectIdentifier storeObject)
                => properties.GetColumnNames(storeObject).SequenceEqual(duplicateProperties.GetColumnNames(storeObject));
        }
    }
}