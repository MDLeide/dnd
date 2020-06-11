namespace DND.Data
{
    // SELECT last_insert_rowid();

    public static class Statements
    {
        /// <summary>
        /// CRUD operations for CardSpaces.
        /// </summary>
        public static class CardSpaces
        {
            public const string SelectAll = @"
SELECT Name,
       Description,
       BackgroundColorCode,
       BackgroundImageId
  FROM CardSpaces;
";

            public const string SelectById = @"
SELECT Name,
       Description,
       BackgroundColorCode,
       BackgroundImageId
  FROM CardSpaces
 WHERE Id = @id;
";

            public const string Insert = @"
INSERT INTO CardSpaces (
                           Name,
                           Description,
                           BackgroundColorCode,
                           BackgroundImageId
                       )
                       VALUES (
                           @name,
                           @description,
                           @backgroundColorCode,
                           @backgroundImageId
                       );

SELECT last_insert_rowid();
";

            public const string Update = @"
UPDATE CardSpace
   SET Name = @name,
       Description = @description,
       BackgroundColorCode = @backgroundColorCode,
       ImageId = @imageId
 WHERE Id = @id;
";

            public const string Delete = @"
DELETE FROM CardSpaces
      WHERE Id = @id;
";
        }


        /// <summary>
        /// CRUD operations for CardSpaceCards.
        /// </summary>
        public static class CardSpaceCards
        {
            public const string SelectByCardAndCardSpaceIds = @"
SELECT CardId,
       CardSpaceId,
       PositionLeft,
       PositionTop,
       ZIndex,
       VisualStateId
  FROM CardSpaceCards
 WHERE CardId = @cardId AND 
       CardSpaceId = @cardSpaceId;
";

            public const string SelectByCardSpaceId = @"
SELECT CardId,
       CardSpaceId,
       PositionLeft,
       PositionTop,
       ZIndex,
       VisualStateId
  FROM CardSpaceCards
 WHERE CardSpaceId = @cardSpaceId;
";

            public const string Insert = @"
INSERT INTO CardSpaceCards (
                               CardId,
                               CardSpaceId,
                               PositionLeft,
                               PositionTop,
                               ZIndex,
                               VisualStateId
                           )
                           VALUES (
                               @cardId,
                               @cardSpaceId,
                               @positionLeft,
                               @positionTop,
                               @zIndex,
                               @visualStateId
                           );
                           
SELECT last_insert_rowid();
";

            public const string Update = @"
UPDATE CardSpaceCards
   SET PositionLeft = @positionLeft,
       PositionRight = @positionRight,
       ZIndex = @zIndex,
       VisualStateId = @visualStateId
 WHERE Id = @id;
";

            public const string Delete = @"
DELETE FROM CardSpaceCards
      WHERE Id = @id;
";

            public const string DeleteByCardAndCardSpaceIds = @"
DELETE FROM CardSpaceCards
      WHERE CardId = @cardId AND 
            CardSpaceId = @cardSpaceId;
";

            public const string DeleteByCardId = @"
DELETE FROM CardSpaceCards
      WHERE CardId = @cardId;
";

            public const string DeleteByCardSpaceID = @"
DELETE FROM CardSpaceCards
      WHERE CardSpaceId = @cardSpaceId;
";
        }

        /// <summary>
        /// Statements for CRUD operations on Images.
        /// </summary>
        public static class Images
        {
            public const string SelectAll = @"
SELECT Id,
       Location,
       Name,
       Description,
       Size
  FROM Images";

            public const string SelectById = @"
SELECT Id,
       Location,
       Name,
       Description,
       Size
  FROM Images
 WHERE Id = @id";

            public const string SelectByCardId = @"
SELECT i.Id,
       i.Location,
       i.Name,
       i.Description,
       i.Size
  FROM Images i
       INNER JOIN
       CardImages ci ON ci.ImageId = i.Id
 WHERE ci.CardId = @id;
";

            public const string Insert = @"
INSERT INTO Images (
                       Location,
                       Name,
                       Description,
                       Size
                   )
                   VALUES (
                       @location,
                       @name,
                       @description,
                       @size
                   );

SELECT last_insert_rowid();
";

            public const string Update = @"
UPDATE Images
   SET Location = @location,
       Name = @name,
       Description = @description,
       Size = @size
 WHERE Id = @id;
";

            public const string Delete = @"
DELETE FROM Images
      WHERE Id = @id;
";
        }


        /// <summary>
        /// Statements for CRUD operations on CardImages.
        /// </summary>
        public static class CardImages
        {
            public const string SelectAll = @"
SELECT Id,
       CardId,
       ImageId
  FROM CardImages;
";

            public const string SelectByCardId = @"
SELECT Id,
       CardId,
       ImageId
  FROM CardImages
 WHERE CardId = @id;
";

            public const string SelectById = @"
SELECT Id,
       CardId,
       ImageId
  FROM CardImages
 WHERE Id = @id;
";

            public const string Exists = @"
SELECT CASE EXISTS (
                SELECT 1
                  FROM CardImages ci
                 WHERE ci.CardId = @cardId AND 
                       ci.ImageId = @imageId
            )
       WHEN 1 THEN 1 ELSE 0 END;
";

            public const string Insert = @"
INSERT INTO CardImages (
                           CardId,
                           ImageId
                       )
                       VALUES (
                           @cardId,
                           @imageId
                       );

SELECT last_insert_rowid();
";

            public const string Update = @"
UPDATE CardImages
   SET CardId = @cardId,
       ImageId = @imageId;
";

            public const string Delete = @"
DELETE FROM CardImages
      WHERE Id = @id;
";

            public const string DeleteByCardId = @"
DELETE FROM CardImages
      WHERE CardId = @id;
";

            public const string DeleteByImageId = @"
DELETE FROM CardImages
      WHERE ImageId = @id;
";
        }

        
        /// <summary>
        /// Statements for CRUD operations on Property Types.
        /// </summary>
        public static class PropertyTypes
        {
            // select
            public const string SelectAll = @"
SELECT Id,
       Name,
       Description
  FROM PropertyTypes pt";

            public const string SelectById = @"
SELECT Id,
       Name,
       Description
  FROM PropertyTypes pt
 WHERE pt.Id = @id;
";

            public const string SelectByCardTypeId = @"
SELECT pt.Id,
       pt.Name,
       pt.Description
  FROM PropertyTypes pt
       INNER JOIN
       PropertyTypeCardTypes ptct ON ptct.PropertyTypeId = pt.Id
 WHERE ptct.CardTypeId = @id;
";

            public const string SelectByName = @"
SELECT Id,
       Name,
       Description
  FROM PropertyTypes pt
 WHERE pt.Name = @name;
";

            public const string Insert = @"
INSERT INTO PropertyTypes (
                              Name,
                              Description
                          )
                          VALUES (
                              @name,
                              @description
                          );

SELECT last_insert_rowid();
";

            public const string Update = @"
UPDATE PropertyTypes
   SET Name = @name,
       Description = @description
 WHERE Id = @id;
";

            public const string Delete = @"
DELETE FROM PropertyTypes 
      WHERE Id = @id;
";
        }
        

        /// <summary>
        /// Statements for CRUD operations on Property Type Card Types.
        /// </summary>
        public static class PropertyTypeCardTypes
        {
            public const string SelectAll = @"
SELECT Id,
       PropertyTypeId,
       CardTypeId
  FROM PropertyTypeCardTypes;
";

            public const string SelectByCardTypeId = @"
SELECT Id,
       PropertyTypeId,
       CardTypeId
  FROM PropertyTypeCardTypes p
 WHERE p.PropertyTypeId = @id;
";

            public const string SelectByPropertyTypeId = @"
SELECT Id,
       PropertyTypeId,
       CardTypeId
  FROM PropertyTypeCardTypes p
 WHERE p.CardTypeId = @id;
";

            public const string Insert = @"
INSERT INTO PropertyTypeCardTypes (
                                      PropertyTypeId,
                                      CardTypeId
                                  )
                                  VALUES (
                                      @propertyTypeId,
                                      @cardTypeId
                                  );

SELECT last_insert_rowid();
";

            public const string Exists = @"
SELECT CASE WHEN EXISTS (
               SELECT 1
                 FROM PropertyTypeCardTypes ptct
                WHERE ptct.CardTypeId = @cardTypeId AND 
                      ptct.PropertyTypeId = @propertyTypeId
           )
       THEN 1 ELSE 0 END;
";

            public const string DeleteByIds = @"
DELETE FROM PropertyTypeCardTypes
      WHERE PropertyTypeId = @propertyTypeId AND
            CardTypeId = @cardTypeId;
";

            public const string Delete = @"
DELETE FROM PropertyTypeCardTypes
      WHERE Id = @id;
";
        }

        
        /// <summary>
        /// Statements for CRUD operations on Properties.
        /// </summary>
        public static class Properties
        {
            public const string SelectByCardId = @"
SELECT Id,
       PropertyTypeId,
       CardId,
       Value
  FROM Properties p
 WHERE p.CardId = @id;
";

            public const string Insert = @"
INSERT INTO Properties (
                           PropertyTypeId,
                           CardId,
                           Value
                       )
                       VALUES (
                           @propertyTypeId,
                           @cardId,
                           @val
                       );
                      
SELECT last_insert_rowid();
";

            public const string Update = @"
UPDATE Properties
   SET PropertyTypeId = @propertyTypeId,
       CardId = @cardId,
       Value = @val;
 WHERE Id = @id";

            public const string Delete = @"
DELETE FROM Properties
      WHERE Id = @id;
";
        }

        
        /// <summary>
        /// Statements for CRUD operations on Hard Links.
        /// </summary>
        public static class HardLinks
        {
            // select
            public const string SelectById = @"
SELECT Id,
       OriginId,
       TargetId,
       Mutual
  FROM HardLinks hl
 WHERE hl.Id = @id;
";

            public const string SelectByOriginId = @"
SELECT Id,
       OriginId,
       TargetId,
       Mutual
  FROM HardLinks hl
 WHERE hl.OriginId = @id;
";

            public const string SelectByTargetId = @"
SELECT Id,
       OriginId,
       TargetId,
       Mutual
  FROM HardLinks hl
 WHERE hl.TargetId = @id;
";

            public const string SelectByEitherCardId = @"
SELECT Id,
       OriginId,
       TargetId,
       Mutual
  FROM HardLinks hl
 WHERE hl.TargetId = @id OR hl.OriginId = @id;
";

            public const string Insert = @"
INSERT INTO HardLinks (
                          OriginId,
                          TargetId,
                          Mutual
                      )
                      VALUES (
                          @originId,
                          @targetId,
                          @mutual
                      );
                      
SELECT last_insert_rowid();
";

            public const string Update = @"
UPDATE HardLinks
   SET OriginId = @originId,
       TargetId = @targetId,
       Mutual = @mututal
 WHERE Id = @id;
";

            public const string Delete = @"
DELETE FROM HardLinks
      WHERE Id = @id;
";
        }
        

        /// <summary>
        /// Statements for CRUD operations on Card Types.
        /// </summary>
        public static class CardTypes
        {
            // select
            public const string SelectById = @"
SELECT Id,
       Name,
       Description,
       ImageId
  FROM CardTypes ct
  WHERE ct.Id = @id;
";

            public const string SelectByName = @"
SELECT Id,
       Name,
       Description,
       ImageId
  FROM CardTypes ct
  WHERE ct.Id = ct.Name = @name;
";

            public const string SelectByPropertyTypeId = @"
SELECT Id,
       Name,
       Description,
       ImageId
  FROM CardTypes ct
       INNER JOIN
       PropertyTypeCardTypes ptct ON ptct.CardTypeId = ct.Id
 WHERE ptct.PropertyTypeId = @id;
";

            public const string SelectAll = @"
SELECT Id,
       Name,
       Description,
       ImageId
  FROM CardTypes;
";
            
            public const string Insert = @"
INSERT INTO CardTypes (
                          Name,
                          Description,
                          ImageId
                      )
                      VALUES (
                          @name,
                          @description,
                          @imageId
                      );
                      
SELECT last_insert_rowid();
";

            public const string Update = @"
UPDATE CardTypes
   SET Name = @name,
       Description = @description,
       ImageId = @imageId
 WHERE Id = @id;
";

            public const string Delete = @"
DELETE FROM CardTypes
      WHERE Id = @id;
";

        }

        
        /// <summary>
        /// Statements for CRUD operations on Cards.
        /// </summary>
        public static class Cards
        {
            // select
            public const string SelectById = @"
SELECT Id,
       Title,
       Subtitle,
       Description,
       CardTypeId
  FROM Cards c
 WHERE c.Id = @id;
";

            public const string SelectByTitle = @"
SELECT Id,
       Title,
       Subtitle,
       Description,
       CardTypeId
  FROM Cards c
 WHERE c.Title = @title;
";

            public const string SelectByTitleStartsWith = @"
SELECT Id,
       Title,
       Subtitle,
       Description,
       CardTypeId
  FROM Cards c
 WHERE (@caseSensitive = 1 AND 
        c.Title LIKE '%' || @title || '%') OR 
       (@caseSensitive = 0 AND 
        LOWER(c.Title) LIKE '%' || LOWER(@title) || '%');
";

            public const string SelectByTitleContains = @"
SELECT Id,
       Title,
       Subtitle,
       Description,
       CardTypeId
  FROM Cards c
 WHERE (@caseSensitive = 1 AND 
        c.Title LIKE '%' || @title || '%') OR 
       (@caseSensitive = 0 AND 
        LOWER(c.Title) LIKE '%' || LOWER(@title) || '%');
";

            public const string SelectAll = @"
SELECT Id,
       Title,
       Subtitle,
       Description,
       CardTypeId
  FROM Cards c;
";
            
            public const string Insert = @"
INSERT INTO Cards (
                      Title,
                      Subtitle,
                      Description,
                      CardTypeId
                  )
                  VALUES (
                      @title,
                      @subtitle,
                      @description,
                      @cardTypeID
                  );

SELECT last_insert_rowid();
";

            public const string Update = @"
UPDATE Cards
   SET Title = @title,
       Subtitle = @subtitle,
       Description = @description,
       CardTypeId = @cardTypeId
 WHERE Id = @id;
";

            public const string Delete = @"
DELETE FROM Cards
      WHERE Id = @id;
";
        }
    }
}