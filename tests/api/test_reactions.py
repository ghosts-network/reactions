from src.api import Api

class TestReactions(Api):
    def test_get_nonexistent_reaction(self):
        resp = self.get_reaction_by_key('nonexistent-id')

        assert resp.status_code == 404

    def test_get_reaction_by_key(self):
        self.post_reaction({'key': 'Post_Test', 'author': 'Test_Author', 'type': 'like'})
        reaction_key = 'Post_Test'

        resp = self.get_reaction_by_key(reaction_key)

        assert resp.status_code == 200

    def test_put_reaction(self):
        reaction = {'key': 'Post_Test', 'author': 'Test_Author', 'type': 'like'}

        self.post_reaction(reaction)

        resp = self.put_reaction({'key': 'Post_Test', 'author': 'Test_Author', 'type': 'wow'})
        resp_body = resp.json()

        assert resp.status_code == 200
        assert resp_body['wow'] > 0

    def test_delete_reaction(self):
        reaction_key = 'Post_Test'
        reaction_author = 'Test_Author'

        self.post_reaction({'key': reaction_key, 'author': reaction_author, 'type': 'like'})

        delete_resp = self.delete_reaction(reaction_key, reaction_author)
        delete_resp_body = delete_resp.json()

        assert delete_resp_body == {}

    def test_delete_nonexistent_reaction(self):
        reaction_key = 'nonexistent-id'
        reaction_author = 'Test_Author'

        resp = self.delete_reaction(reaction_key, reaction_author)

        assert resp.status_code == 404

    # reported to https://github.com/ghosts-network/reactions/issues/16
    def test_post_exist_new_type_reaction(self):
        reaction = {'key': 'Post_Test', 'author': 'Test_Author', 'type': 'like'}

        first_resp = self.post_reaction(reaction).json()

        reaction['type'] = 'wow'
        second_resp = self.post_reaction(reaction).json()

        assert 'like' in first_resp and 'wow' in second_resp
