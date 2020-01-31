(defblock :name curl :is-top t
	(worker
		:worker-name send
		:verb "send"
		:description "Sends message to a named subscriber."
		:usage-samples (
			"send -t alice -s 'Some stuff to do' -c 'let`s go party!' "
			"send --to bob --subject 'Thanks Bob' --content 'The party was great!' "))


	(multi-text
		:classes key
		:values "-t" "--to"
		:alias to
		:action key)

	(some-text
		:classes term
		:action value
		:description "Recipient"
		:doc-subst "recipient")

	(multi-text
		:classes key
		:values "-s" "--subject"
		:alias subject
		:action key)

	(some-text
		:classes string
		:action value
		:description "Subject"
		:doc-subst "subject")

	(multi-text
		:classes key
		:values "-c" "--content"
		:alias content
		:action key)

	(some-text
		:classes string
		:action value
		:description "Content"
		:doc-subst "content")

	(end)
)
