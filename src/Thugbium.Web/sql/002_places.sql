CREATE TABLE places (
    id bigint GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    owner_user_id uuid NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    name varchar(100) NOT NULL,
    description varchar(1000) NOT NULL DEFAULT '',
    visibility varchar(16) NOT NULL DEFAULT 'Private' CHECK (visibility IN ('Private', 'Public')),
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz NOT NULL DEFAULT now()
);

CREATE INDEX places_owner_user_id_idx ON places(owner_user_id);
CREATE INDEX places_public_updated_at_idx ON places(visibility, updated_at DESC);
