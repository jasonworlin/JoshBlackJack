-- SQLite
-- select * from users
-- select sql from sqlite_master where tbl_name = 'Players';

SELECT "t"."GameId", "t"."DealerId", "t"."DeckId", "t"."PlayerId", "t"."DeckId0", "t"."DealerId0", "t"."HandId0", "t"."PlayerId0", "t"."HandId1", "t0"."BotId", "t0"."GameId", "t0"."Hand1HandId", "t0"."Hand2HandId", "t0"."HasBusted", "t0"."HasSplit", "t0"."HasStuck", "t0"."HasWon", "t0"."HandId", "t0"."HandId0", "t0"."CardId", "t0"."DeckId", "t0"."HandId1", "t0"."Suit", "t0"."Value", "t0"."CardId0", "t0"."DeckId0", "t0"."HandId2", "t0"."Suit0", "t0"."Value0", "c1"."CardId", "c1"."DeckId", "c1"."HandId", "c1"."Suit", "c1"."Value", "t"."HandId", "t"."HasBusted", "t"."HasSplit", "t"."HasStuck", "t"."Total", "c2"."CardId", "c2"."DeckId", "c2"."HandId", "c2"."Suit", "c2"."Value", "t"."Balance", "t"."CanSplit", "t"."Hand1HandId", "t"."Hand2HandId", "t"."HasBusted0", "t"."HasStuck0", "t"."HasWon", "t"."UserId", "c3"."CardId", "c3"."DeckId", "c3"."HandId", "c3"."Suit", "c3"."Value"
      FROM (
          SELECT "g"."GameId", "g"."DealerId", "g"."DeckId", "g"."PlayerId", "d"."DeckId" AS "DeckId0", "d0"."DealerId" AS "DealerId0", "d0"."HandId", "d0"."HasBusted", "d0"."HasSplit", "d0"."HasStuck", "d0"."Total", "h"."HandId" AS "HandId0", "p"."PlayerId" AS "PlayerId0", "p"."Balance", "p"."CanSplit", "p"."Hand1HandId", "p"."Hand2HandId", "p"."HasBusted" AS "HasBusted0", "p"."HasStuck" AS "HasStuck0", "p"."HasWon", "p"."UserId", "h0"."HandId" AS "HandId1"
          FROM "Games" AS "g"
          INNER JOIN "Decks" AS "d" ON "g"."DeckId" = "d"."DeckId"
          INNER JOIN "Dealers" AS "d0" ON "g"."DealerId" = "d0"."DealerId"
          INNER JOIN "Hands" AS "h" ON "d0"."HandId" = "h"."HandId"
          INNER JOIN "Players" AS "p" ON "g"."PlayerId" = "p"."PlayerId"
          INNER JOIN "Hands" AS "h0" ON "p"."Hand1HandId" = "h0"."HandId"
          WHERE "g"."GameId" = 1
          LIMIT 2
      ) AS "t"
      LEFT JOIN (
          SELECT "b"."BotId", "b"."GameId", "b"."Hand1HandId", "b"."Hand2HandId", "b"."HasBusted", "b"."HasSplit", "b"."HasStuck", "b"."HasWon", "h1"."HandId", "h2"."HandId" 
AS "HandId0", "c"."CardId", "c"."DeckId", "c"."HandId" AS "HandId1", "c"."Suit", "c"."Value", "c0"."CardId" AS "CardId0", "c0"."DeckId" AS "DeckId0", "c0"."HandId" AS "HandId2", "c0"."Suit" AS "Suit0", "c0"."Value" AS "Value0"
          FROM "Bots" AS "b"
          INNER JOIN "Hands" AS "h1" ON "b"."Hand1HandId" = "h1"."HandId"
          INNER JOIN "Hands" AS "h2" ON "b"."Hand2HandId" = "h2"."HandId"
          LEFT JOIN "Cards" AS "c" ON "h1"."HandId" = "c"."HandId"
          LEFT JOIN "Cards" AS "c0" ON "h2"."HandId" = "c0"."HandId"
      ) AS "t0" ON "t"."GameId" = "t0"."GameId"
      LEFT JOIN "Cards" AS "c1" ON "t"."DeckId0" = "c1"."DeckId"
      LEFT JOIN "Cards" AS "c2" ON "t"."HandId0" = "c2"."HandId"
      LEFT JOIN "Cards" AS "c3" ON "t"."HandId1" = "c3"."HandId"
      ORDER BY "t"."GameId", "t"."DeckId0", "t"."DealerId0", "t"."HandId0", "t"."PlayerId0", "t"."HandId1", "t0"."BotId", "t0"."HandId", "t0"."HandId0", "t0"."CardId", "t0"."CardId0", "c1"."CardId", "c2"."CardId"