-- Adds sequential public profile IDs (1, 2, 3...) without replacing internal UUID account IDs.
CREATE SEQUENCE IF NOT EXISTS users_profile_id_seq;

ALTER TABLE users ADD COLUMN IF NOT EXISTS profile_id bigint;
ALTER TABLE users ALTER COLUMN profile_id SET DEFAULT nextval('users_profile_id_seq');

UPDATE users
SET profile_id = nextval('users_profile_id_seq')
WHERE profile_id IS NULL;

ALTER SEQUENCE users_profile_id_seq OWNED BY users.profile_id;
SELECT setval('users_profile_id_seq', COALESCE((SELECT MAX(profile_id) FROM users), 0) + 1, false);

ALTER TABLE users ALTER COLUMN profile_id SET NOT NULL;
CREATE UNIQUE INDEX IF NOT EXISTS users_profile_id_key ON users(profile_id);
