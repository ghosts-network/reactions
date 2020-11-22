import requests
import json

class Api:
  def get_reaction_by_key(self, key):
    url = 'http://localhost:5000/reactions/' + key
    headers = {'Content-Type': 'application/json' }

    return requests.get(url, headers = headers)

  def post_reaction(self, body):
    url = 'http://localhost:5000/reactions/{key}/{type}'.format(
        key = body['key'], type = body['type']
    )
    headers = {'Content-Type': 'application/json', 'Author': body['author'] }

    return requests.post(url, headers = headers, data = json.dumps(body, indent = 4))
    
  def put_reaction(self, body):
    url = 'http://localhost:5000/reactions/{key}/{type}'.format(
        key = body['key'], type = body['type']
    )
    headers = {'Content-Type': 'application/json', 'Author': body['author'] }

    return requests.put(url, headers = headers, data = json.dumps(body, indent = 4))

  def delete_reaction(self, key, author):
    url = 'http://localhost:5000/reactions/' + key
    headers = {'Content-Type': 'application/json', 'Author': author }

    return requests.delete(url, headers = headers)