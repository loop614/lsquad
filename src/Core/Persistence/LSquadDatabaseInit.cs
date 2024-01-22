using Dapper;
using Npgsql;

namespace Lsquad.Core.Persistence;

class LsquadCoreDatabaseInit
{
    public async static Task InitTables(NpgsqlConnection con) {
        var sql = """
            CREATE TABLE IF NOT EXISTS
            lsquad_setting (
                id_setting serial PRIMARY KEY,
                name VARCHAR (255),
                status INTEGER,
                UNIQUE(name)
            );

            INSERT INTO lsquad_setting (name, status)
            VALUES ('br_domain_player', 0)
            ON CONFLICT (name) DO NOTHING;

            INSERT INTO lsquad_setting (name, status)
            VALUES ('br_domain_squad', 0)
            ON CONFLICT (name) DO NOTHING;

            INSERT INTO lsquad_setting (name, status)
            VALUES ('br_domain_team', 0)
            ON CONFLICT (name) DO NOTHING;

            CREATE TABLE IF NOT EXISTS
            lsquad_language (
                id_language serial PRIMARY KEY,
                name VARCHAR (255),
                UNIQUE(name)
            );

            INSERT INTO lsquad_language (name)
            VALUES('en')
            ON CONFLICT (name) DO NOTHING;

            CREATE TABLE IF NOT EXISTS
            lsquad_team (
                id_team serial PRIMARY KEY,
                external_team_id INTEGER NOT NULL,
                UNIQUE(external_team_id),
                created_at timestamp DEFAULT now(),
                updated_at timestamp
            );
            CREATE INDEX IF NOT EXISTS idx_lsquad_player_external_team_id ON lsquad_team(external_team_id);

            CREATE TABLE IF NOT EXISTS
            lsquad_team_name (
                id_team_name serial PRIMARY KEY,
                fk_language INTEGER NOT NULL,
                FOREIGN KEY (fk_language) REFERENCES lsquad_language (id_language),
                fk_team INTEGER,
                FOREIGN KEY (fk_team) REFERENCES lsquad_team (id_team),
                name VARCHAR (255),
                version REAL NOT NULL,
                UNIQUE(fk_team, fk_language),
                created_at timestamp DEFAULT now(),
                updated_at timestamp
            );

            CREATE TABLE IF NOT EXISTS
            lsquad_player (
                id_player serial PRIMARY KEY,
                fk_team INTEGER,
                FOREIGN KEY (fk_team) REFERENCES lsquad_team (id_team),
                version REAL,
                weight VARCHAR (255),
                shirt_number VARCHAR (255),
                preferred_foot INTEGER,
                position INTEGER,
                external_player_id INTEGER,
                UNIQUE(external_player_id),
                last_name VARCHAR (255),
                height VARCHAR (255),
                full_name VARCHAR (255),
                first_name VARCHAR (255),
                country_code VARCHAR (255),
                birth_date DATE,
                created_at timestamp DEFAULT now(),
                updated_at timestamp
            );
            CREATE INDEX IF NOT EXISTS idx_lsquad_player_external_player_id ON lsquad_player(external_player_id);

            CREATE TABLE IF NOT EXISTS
            lsquad_player_name (
                id_player_name serial PRIMARY KEY,
                fk_language INTEGER NOT NULL,
                fk_player INTEGER NOT NULL,
                FOREIGN KEY (fk_language) REFERENCES lsquad_language (id_language),
                FOREIGN KEY (fk_player) REFERENCES lsquad_player (id_player),
                version REAL NOT NULL,
                UNIQUE(fk_player, fk_language),
                name VARCHAR (255),
                created_at timestamp DEFAULT now(),
                updated_at timestamp
            );
            CREATE INDEX IF NOT EXISTS idx_lsquad_lsquad_player_name_name ON lsquad_player_name(name);
        """;

        await con.ExecuteAsync(sql);
    }
}
