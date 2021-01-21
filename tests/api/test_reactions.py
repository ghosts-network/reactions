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

    def test_get_reaction_by_author(self):
        key = 'Post_Test'

        self.post_reaction({'key': key, 'author': 'Test_Author', 'type': 'like'})
        self.post_reaction({'key': key, 'author': 'Test_Author2', 'type': 'wow'})

        resp = self.get_reaction_by_author(key, 'Test_Author')
        resp_body = resp.json()

        assert resp.status_code == 200 and resp_body['type'] == 'like'

    def test_get_nonexistent_reaction_by_author(self):
        self.post_reaction({'key': 'Post_Test', 'author': 'Test_Author', 'type': 'like'})

        resp = self.get_reaction_by_author('Post_Test', 'nonexistent-id')
        
        assert resp.status_code == 404

    def test_get_grouped_reactions(self):
        self.post_reaction({'key': 'Post_Test', 'author': 'Test_Author', 'type': 'wow'})
        self.post_reaction({'key': 'Post_Test', 'author': 'Test_Author2', 'type': 'like'})
        self.post_reaction({'key': 'Post_Test2', 'author': 'Test_Author', 'type': 'wow'})
        
        body = {'publicationIds': ['Post_Test', 'Post_Test2']}

        resp = self.get_grouped_reactions(body)
        resp_body = resp.json()
        
        first_publication = resp_body['Post_Test']
        second_publication = resp_body['Post_Test2']

        assert resp.status_code == 200 and ('Post_Test' in resp_body) and ('Post_Test2' in resp_body)
        assert ('like' in first_publication) and ('wow' in first_publication) and ('wow' in second_publication)

    def test_post_reaction(self):
        reaction = {'key': 'Post_Test', 'author': 'Test_Author', 'type': 'like'}

        self.post_reaction(reaction)

        resp = self.post_reaction({'key': 'Post_Test', 'author': 'Test_Author', 'type': 'wow'})
        resp_body = resp.json()

        assert resp.status_code == 201
        assert resp_body['wow'] > 0

    def test_delete_by_author_reaction(self):
        reaction_key = 'Post_Test'
        reaction_author = 'Test_Author'

        self.post_reaction({'key': reaction_key, 'author': reaction_author, 'type': 'like'})

        delete_resp = self.delete_reaction_by_author(reaction_key, reaction_author)

        assert delete_resp.status_code == 200

    def test_delete_by_author_nonexistent_reaction(self):
        reaction_key = 'nonexistent-id'
        reaction_author = 'Test_Author'

        resp = self.delete_reaction_by_author(reaction_key, reaction_author)

        assert resp.status_code == 404

    # reported to https://github.com/ghosts-network/reactions/issues/16
    def test_post_exist_new_type_reaction(self):
        reaction = {'key': 'Post_Test', 'author': 'Test_Author', 'type': 'like'}

        first_resp = self.post_reaction(reaction).json()

        reaction['type'] = 'wow'
        second_resp = self.post_reaction(reaction).json()

        assert 'like' in first_resp and 'wow' in second_resp

    def test_delete_all_reactions(self):
        reaction_key = 'Post_Test'

        like_reaction = {'key': reaction_key, 'author': 'Test_Author1', 'type': 'like'}
        wow_reaction = {'key': reaction_key, 'author': 'Test_Author2', 'type': 'wow'}

        self.post_reaction(like_reaction)
        self.post_reaction(wow_reaction)

        response = self.delete_all_reactions(reaction_key)
        stats = response.json()

        assert response.status_code == 200 and stats == {}

    def test_delete_all_nonexistent_reaction(self):
        reaction_key = 'nonexistent-id'

        resp = self.delete_all_reactions(reaction_key)
        stats = resp.json()

        assert resp.status_code == 200 and stats == {}